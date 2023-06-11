using Microsoft.EntityFrameworkCore;
using SharedLib;

namespace ab.context
{
    /// <summary>
    /// Контекст БД хранимых параметров
    /// </summary>
    public class StorageParametersContext : DbContext
    {
        /// <summary>
        /// Атрибут блокировки (для lock {}).
        /// В частности SQLite на слабом Android железе иногда не поспевала записывать данные
        /// </summary>
        private static object DbLocker = new();
        /// <summary>
        /// Хранимые значения параметров (в БД)
        /// </summary>
        public DbSet<ParametersStorageModelDB> ParametersStorage { get; set; }


        /// <inheritdoc/>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connect_string = $"Filename={GlobalStatic.ParametersStorageDatabasePath}";
            System.Diagnostics.Debug.WriteLine(connect_string);
            optionsBuilder.UseSqlite(connect_string);
            base.OnConfiguring(optionsBuilder);
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
        /// <summary>
        /// Контекст БД клиента удалённого управления
        /// </summary>
        public StorageParametersContext()
        {
            Database.Migrate();
        }
    }
}