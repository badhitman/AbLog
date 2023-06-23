namespace SharedLib;

/// <summary>
/// Email отправка/получение
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Отправить сообщение
    /// </summary>
    /// <param name="email">Получатель</param>
    /// <param name="subject">Тема сообщения</param>
    /// <param name="message">Текст сообщения</param>
    /// <param name="html_format">html формат если true. в противном случае TextColor/PlainText</param>
    /// <param name="conf">Конфигурация подключения SMTP/IMAP</param>
    public Task<ResponseBaseModel> SendEmailAsync(string email, string subject, string message, bool html_format, EmailConfigModel conf);

    /// <summary>
    /// Подключиться к SMTP серверу
    /// </summary>
    /// <param name="conf">Конфигурация подключения SMTP/IMAP</param>
    public Task<ResponseBaseModel> ConnectSmtpAsync(EmailConfigModel conf);

    /// <summary>
    /// Подключиться к IMAP серверу
    /// </summary>
    /// <param name="conf">Конфигурация подключения SMTP/IMAP</param>
    public Task<ResponseBaseModel> ConnectImapAsync(EmailConfigModel conf);
}