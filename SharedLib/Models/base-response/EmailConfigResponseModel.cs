////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Настройки Email
/// </summary>
public class EmailConfigResponseModel : ResponseBaseModel
{
    /// <summary>
    /// Настройки Email
    /// </summary>
    public EmailConfigModel? Conf { get; set; }
}