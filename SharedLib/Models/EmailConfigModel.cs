////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace SharedLib;

/// <summary>
/// Конфигурация подключения Email
/// </summary>
public class EmailConfigModel
{
    /// <inheritdoc/>
    [Required]
    public string? Email { get; set; }

    /// <inheritdoc/>
    [Required]
    public string? Login { get; set; }

    /// <inheritdoc/>
    [Required]
    public string? Password { get; set; }


    /// <summary>
    /// Адрес хоста SMTP (исходящая почта)
    /// </summary>
    [Required]
    public string? HostSmtp { get; set; }
    /// <summary>
    /// Порт хоста SMTP (исходящая почта)
    /// </summary>
    [Required]
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
        !string.IsNullOrWhiteSpace(HostSmtp) &&
        MailAddress.TryCreate(Email, out _);

    /// <summary>
    /// Оператор равенства
    /// </summary>
    public static bool operator ==(EmailConfigModel l, EmailConfigModel r)
    {
        return
            l.Email == r.Email &&
            l.Login == r.Login &&
            l.Password == r.Password &&
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
    public override int GetHashCode() => $"{Login}{Password}{HostSmtp}{PortSmtp}{UseSslSmtp}{LogsSmtp}".GetHashCode();
}