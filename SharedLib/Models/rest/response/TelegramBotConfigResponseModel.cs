////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Настройки Telegram bot: Ответ rest/api
/// </summary>
public class TelegramBotConfigResponseModel : ResponseBaseModel
{
    /// <summary>
    /// Настройки
    /// </summary>
    public TelegramBotConfigModel? Conf { get; set; }
}