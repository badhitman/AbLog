﻿////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using SharedLib;

namespace ab.context
{
    /// <summary>
    /// Контекст сервера
    /// </summary>
    public class ServerContext : DbContext
    {
        /// <summary>
        /// Атрибут блокировки (для lock {}).
        /// В частности SQLite на слабом Android железе иногда не поспевала записывать данные
        /// </summary>
        public static object DbLocker = new();

        /// <summary>
        /// Управляющие блоки (устройства)
        /// </summary>
        public DbSet<HardwareModelDB> Hardwares { get; set; }

        /// <summary>
        /// Порты устройства
        /// </summary>
        public DbSet<PortModelDB> Ports { get; set; }

        /// <summary>
        /// Скрипты (пакеты команд)
        /// </summary>
        public DbSet<ScriptModelDB> Scripts { get; set; }

        /// <summary>
        /// Команды скриптов
        /// </summary>
        public DbSet<CommandModelDB> Commands { get; set; }

        /// <summary>
        /// Задачи скриптов
        /// </summary>
        public DbSet<TaskModelDB> Tasks { get; set; }

        /// <summary>
        /// Отчёты выполнния этапов задач скриптов
        /// </summary>
        public DbSet<ReportModelDB> Reports { get; set; }

        /// <summary>
        /// Запрещающее условия (ограничения) для выполнения команд.
        /// </summary>
        public DbSet<CommandConditionModelDB> ConditionsCommands { get; set; }

        /// <summary>
        /// Условия/причины запуска тригеров
        /// </summary>
        public DbSet<TrigerConditionModelDB> TrigersConditions { get; set; }

        /// <summary>
        /// Тригеры/события запуска скриптов
        /// </summary>
        public DbSet<TrigerModelDB> Trigers { get; set; }

        /// <summary>
        /// Конкуренция (взаимоблокировка выполнения скриптов)
        /// </summary>
        public DbSet<ContentionsModelDB> Contentions { get; set; }

        /// <summary>
        /// Хранимые значения параметров (в БД)
        /// </summary>
        public DbSet<ParametersStorageModelDB> ParametersStorage { get; set; }

        /// <inheritdoc/>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connect_string = $"Filename={GlobalStatic.MainDatabasePath}";
            System.Diagnostics.Debug.WriteLine(connect_string);
            optionsBuilder.UseSqlite(connect_string);
            base.OnConfiguring(optionsBuilder);
        }

        /// <summary>
        /// Контекст базы данных сервера
        /// </summary>
        public ServerContext() : base()
        {
            Database.Migrate();
        }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContentionsModelDB>()
            .HasOne(p => p.MasterScript)
            .WithMany(t => t!.Contentions)
            .HasForeignKey(p => p.MasterScriptId)
            .HasPrincipalKey(t => t!.Id);
        }

        /// <summary>
        /// Пересортировка команд в скриптах, которые имеют дробное значение сортировки - с целью установки целых значений
        /// </summary>
        public void ReorderScriptsCommands()
        {
            IQueryable<int> scripts_ids = Commands.Where(x => !EF.Functions.Like(x.Sorting.ToString(), "%.0")).Select(x => x.ScriptId).Distinct().AsQueryable();
            ScriptModelDB[] scripts = Scripts.Where(x => scripts_ids.Contains(x.Id)).Include(x => x.Commands).ToArray();

            if (scripts.Any())
            {
                foreach (ScriptModelDB script in scripts)
                {
                    double d = 1;
                    script.Commands = script.Commands!.OrderBy(x => x.Sorting).Select(x => { x.Sorting = d++; return x; }).ToList();
                }
                UpdateRange(scripts.SelectMany(x => x.Commands!));
                SaveChanges();
            }
        }

        /// <summary>
        /// DEMO данные в БД
        /// </summary>
        public void DemoSeed()
        {
            if (!Hardwares.Any() && !Ports.Any())
            {
                Hardwares.Add(new HardwareModelDB { Name = "DEMO 1", Address = "192.168.0.14", Password = "sec", AlarmSubscriber = false, CommandsAllowed = true });
                SaveChanges();

                for (int i = 1; i <= 38; i++)
                    Ports.AddRange(new PortModelDB() { HardwareId = 1, PortNumb = i });

                SaveChanges();
            }
            if (!Scripts.Any() && !Contentions.Any() && !Commands.Any() && !ConditionsCommands.Any())
            {
                Scripts.Add(new ScriptModelDB { Name = "Утро" });
                Scripts.Add(new ScriptModelDB { Name = "Полив" });
                SaveChanges();

                Contentions.Add(new ContentionsModelDB() { MasterScriptId = 1, SlaveScriptId = 2 });

                Commands.Add(new CommandModelDB { Name = "Выключить уличный свет", Sorting = 0, CommandType = TypesCommandsEnum.Controller, ScriptId = 1, Execution = 2, ExecutionParametr = "15:0;10:0", Hidden = true });
                Commands.Add(new CommandModelDB { Name = "Включить полив", Sorting = 1, CommandType = TypesCommandsEnum.Port, ScriptId = 1, Execution = 30, ExecutionParametr = "on" });
                SaveChanges();

                ConditionsCommands.Add(new CommandConditionModelDB() { OwnerId = 1, Name = "condition #1", HardwareId = 1, PortId = 1 });
                ConditionsCommands.Add(new CommandConditionModelDB() { OwnerId = 1, Name = "condition #2", HardwareId = 2, PortId = 39 });
                SaveChanges();
            }
        }

        /// <summary>
        /// Получить значение параметра
        /// </summary>
        /// <typeparam name="T">Тип значения параметра</typeparam>
        /// <param name="name">Имя параметра</param>
        /// <param name="default_value">Значение параметра по умолчанию (если в БД его нет)</param>
        /// <returns>Значение параметра (или значение по умолчанию, если не найден)</returns>
        public ParametersStorageModelDB GetStoredParameter<T>(string name, T default_value)
        {
            name = name.Trim();
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");

            string t_name = typeof(T).Name.ToLower();

            ParametersStorageModelDB? dataRow;
            lock (DbLocker)
            {
                dataRow = ParametersStorage.FirstOrDefault(x => x.Name.ToLower() == name.ToLower() && x.TypeName.ToLower() == t_name);
                if (dataRow is null)
                {
                    dataRow = new ParametersStorageModelDB()
                    {
                        Name = name,
                        TypeName = typeof(T).Name,
                        StoredValue = default_value?.ToString() ?? string.Empty
                    };
                    ParametersStorage?.Add(dataRow);
                    SaveChanges();
                }
            }
            return dataRow;
        }

        /// <summary>
        /// Сохранить значение параметра в БД
        /// </summary>
        /// <typeparam name="T">Тип значения параметра</typeparam>
        /// <param name="name">Имя параметра</param>
        /// <param name="value">Значение параметра</param>
        public ParametersStorageModelDB SetStoredParameter<T>(string name, T value)
        {
            name = name.Trim();
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");

            string t_name = typeof(T).Name.ToLower();
            ParametersStorageModelDB? dataRow;
            lock (DbLocker)
            {
                dataRow = ParametersStorage.FirstOrDefault(x => x.Name.ToLower() == name.ToLower() && x.TypeName.ToLower() == t_name);
                if (dataRow is null)
                {
                    dataRow = new ParametersStorageModelDB() { Name = name, StoredValue = value?.ToString() ?? string.Empty, TypeName = t_name };
                    ParametersStorage?.Add(dataRow);
                }
                else
                {
                    dataRow.StoredValue = value?.ToString() ?? string.Empty;
                    ParametersStorage?.Update(dataRow);
                }

                SaveChanges();
            }
            return dataRow;
        }
    }
}