////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// 
/// </summary>
public class TelegramBotCheckResponseModel : ResponseBaseModel
{
    /// <summary>
    /// 
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Запущенный бот (текущий демон). Null - если не запущен
    /// </summary>
    public TelegramBotCheckResponseModel? ServiceIsRunning { get; set; }
}