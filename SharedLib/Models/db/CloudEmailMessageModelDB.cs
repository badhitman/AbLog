////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Email сообщение
/// </summary>
public class CloudEmailMessageModelDB : EntryModel
{
    /// <summary>
    /// Идентификатор Email письма
    /// </summary>
    public string UIDL { get; set; } = string.Empty;

    /// <summary>
    /// Тема сообщения
    /// </summary>
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    /// Тело сообщения
    /// </summary>
    public string Body { get; set; } = string.Empty;

    /// <summary>
    /// Атрибуты
    /// </summary>
    public string Extra { get; set; } = string.Empty;
}