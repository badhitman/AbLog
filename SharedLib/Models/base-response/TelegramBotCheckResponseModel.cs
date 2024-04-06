////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Результат проверки текущего бота
/// </summary>
public class TelegramBotCheckResponseModel : ResponseBaseModel
{
    /// <summary>
    /// Id - TelegramBot
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// FirstName - TelegramBot
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// LastName - TelegramBot
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Username - TelegramBot
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Запущенный бот (текущий демон). Null - если не запущен
    /// </summary>
    public TelegramBotCheckResponseModel? ServiceIsRunning { get; set; }
}