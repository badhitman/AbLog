////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using SharedLib;

namespace ab.context
{
    /// <summary>
    /// Контекст базы данных логов
    /// </summary>
    public class LogsContext : DbContext
    {
        /// <summary>
        /// Атрибут блокировки (для lock {}).
        /// В частности SQLite на слабом Android железе иногда не поспевала записывать данные
        /// </summary>
        static object DbLocker = new();

        /// <summary>
        /// Логи
        /// </summary>
        public DbSet<LogRowModelDB> Logs { get; set; }

        /// <summary>
        /// Контекст базы данных логов
        /// </summary>
        public LogsContext()
        {
            if (OperatingSystem.IsAndroid())
                Database.EnsureCreated();
            else
                Database.Migrate();
        }

        /// <summary>
        /// Добавить лог в базу данных
        /// </summary>
        /// <param name="logStatus">Статус лога</param>
        /// <param name="Message">Сообщение</param>
        /// <param name="tag">Маркер</param>
        public LogRowModelDB AddLogRow(LogStatusesEnum logStatus, string Message, string tag)
        {
            if (tag.StartsWith("●"))
            {
                tag = tag[1..].TrimStart();
            }
            LogRowModelDB logRow = new() { Status = logStatus, Name = Message, TAG = tag };
            lock (DbLocker)
            {
                Logs?.Add(logRow);
                try
                {
                    SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return logRow;
        }

        /// <inheritdoc/>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={GlobalStatic.LogsDatabasePath}");
            base.OnConfiguring(optionsBuilder);
        }
    }
}