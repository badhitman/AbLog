using System.Net.Mail;

namespace SharedLib
{
    /// <summary>
    /// Конфигурация подключения Email
    /// </summary>
    public class EmailConfigModel
    {
        /// <summary>
        /// Имя адреса
        /// </summary>
        public string Name { get; set; } = default!;

        /// <summary>
        /// Email адрес
        /// </summary>
        public string Email { get; set; } = default!;

        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; } = default!;

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; } = default!;


        /// <summary>
        /// Адрес хоста IMAP (вкодящая почта)
        /// </summary>
        public string? HostImap { get; set; }
        /// <summary>
        /// Порт хоста IMAP (вкодящая почта)
        /// </summary>
        public int PortImap { get; set; } = 993;
        /// <summary>
        /// Использовать SSL IMAP (вкодящая почта)
        /// </summary>
        public SecureSocketOptionsEnum UseSslImap { get; set; } = SecureSocketOptionsEnum.None;
        /// <summary>
        /// Включить логирование для IMAP клиента
        /// </summary>
        public bool LogsImap { get; set; }


        /// <summary>
        /// Адрес хоста SMTP (исходящая почта)
        /// </summary>
        public string? HostSmtp { get; set; }
        /// <summary>
        /// Порт хоста SMTP (исходящая почта)
        /// </summary>
        public int PortSmtp { get; set; } = 465;
        /// <summary>
        /// Использовать SSL SMTP (исходящая почта)
        /// </summary>
        public SecureSocketOptionsEnum UseSslSmtp { get; set; } = SecureSocketOptionsEnum.None;
        /// <summary>
        /// Включить логирование для SMTP клиента
        /// </summary>
        public bool LogsSmtp { get; set; }


        /// <summary>
        /// Конфигурация установлена
        /// </summary>
        public virtual bool IsConfigured =>
            !string.IsNullOrWhiteSpace(Login) &&
            !string.IsNullOrWhiteSpace(Password) &&
            !string.IsNullOrWhiteSpace(HostImap) &&
            !string.IsNullOrWhiteSpace(HostSmtp) &&
            MailAddress.TryCreate(Email, out _);

        /// <summary>
        /// Опратор равенства
        /// </summary>
        public static bool operator ==(EmailConfigModel l, EmailConfigModel r)
        {
            return
                l.Name == r.Name &&
                l.Email == r.Email &&
                l.Login == r.Login &&
                l.Password == r.Password &&
                l.HostImap == r.HostImap &&
                l.PortImap == r.PortImap &&
                l.UseSslImap == r.UseSslImap &&
                l.LogsImap == r.LogsImap &&
                l.HostSmtp == r.HostSmtp &&
                l.PortSmtp == r.PortSmtp &&
                l.UseSslSmtp == r.UseSslSmtp &&
                l.LogsSmtp == r.LogsSmtp;
        }

        /// <summary>
        /// Оператор НЕ-равенства
        /// </summary>
        public static bool operator !=(EmailConfigModel l, EmailConfigModel r) => !(l == r);

        /// <summary>
        /// Equals
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is EmailConfigModel ec)
            {
                return this == ec;
            }
            else
                return false;
        }

        /// <summary>
        /// GetHashCode
        /// </summary>
        public override int GetHashCode() => $"{Login}{Password}{HostImap}{PortImap}{UseSslImap}{LogsImap}{HostSmtp}{PortSmtp}{UseSslSmtp}{LogsSmtp}".GetHashCode();
    }
}