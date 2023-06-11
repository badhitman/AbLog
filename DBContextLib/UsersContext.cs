////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using SharedLib;

namespace ab.context
{
    /// <summary>
    /// Контекст пользователей и сообщений
    /// </summary>
    public class UsersContext : DbContext
    {
        /// <summary>
        /// Атрибут блокировки (для lock {}).
        /// В частности SQLite на слабом Android железе иногда не поспевала записывать данные
        /// </summary>
        public static object DbLocker = new();

        /// <summary>
        /// Пользователи системы
        /// </summary>
        public DbSet<UserModelDB> Users { get; set; }

        /// <summary>
        /// Email сообщения
        /// </summary>
        public DbSet<CloudEmailMessageModelDB> EmailMessages { get; set; }

        /// <summary>
        /// Сообщения Telegram
        /// </summary>
        public DbSet<TelegramMessageModelDB> TelegramMessages { get; set; }

        /// <summary>
        /// Пользователи Telegram
        /// </summary>
        public DbSet<TelegramUserModelDB> TelegramUsers { get; set; }

        /// <inheritdoc/>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connect_string = $"Filename={GlobalStatic.UsersDatabasePath}";
            System.Diagnostics.Debug.WriteLine(connect_string);
            optionsBuilder.UseSqlite(connect_string);
            base.OnConfiguring(optionsBuilder);
        }

        /// <summary>
        /// Контекст базы данных сервера
        /// </summary>
        public UsersContext() : base()
        {
            Database.Migrate();
        }

        /// <summary>
        /// DEMO данные в БД
        /// </summary>
        public void DemoSeed()
        {
            if (!Users.Any())
            {
                Users.Add(new UserModelDB { Name = "Tom", Email = "tom@gmail.com", Phone = "+79995554422", AlarmSubscriber = true, CommandsAllowed = true });
                Users.Add(new UserModelDB { Name = "Alice", Email = "alice@gmail.com", Phone = "+75556664411", AlarmSubscriber = false, CommandsAllowed = true });
                SaveChanges();
            }
        }
    }
}