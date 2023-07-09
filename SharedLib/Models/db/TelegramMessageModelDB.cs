////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Сообщение Telegram
/// </summary>
public class TelegramMessageModelDB : EntryModel
{
    /// <summary>
    /// Идентификатор сообщения
    /// </summary>
    public string MessageId { get; set; } = default!;

    /// <summary>
    /// Идентификатор отправителя сообщения
    /// </summary>
    public string UserSenderId { get; set; } = default!;

    /// <summary>
    /// Идентификатр чата сообщения
    /// </summary>
    public string ChatId { get; set; } = default!;
}