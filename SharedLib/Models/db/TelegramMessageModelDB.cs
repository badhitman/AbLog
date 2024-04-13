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
    public required string MessageId { get; set; }

    /// <summary>
    /// Идентификатор отправителя сообщения
    /// </summary>
    public required string UserSenderId { get; set; }

    /// <summary>
    /// Идентификатр чата сообщения
    /// </summary>
    public required string ChatId { get; set; }
}