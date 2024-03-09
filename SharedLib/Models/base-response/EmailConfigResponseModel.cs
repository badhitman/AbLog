////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Настройки Email: Ответ rest/api
/// </summary>
public class EmailConfigResponseModel : ResponseBaseModel
{
    /// <summary>
    /// Порт устройства
    /// </summary>
    public EmailConfigModel? Conf { get; set; }
}