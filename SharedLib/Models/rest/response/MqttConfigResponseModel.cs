////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Настройки MQTT: Ответ rest/api
/// </summary>
public class MqttConfigResponseModel : ResponseBaseModel
{
    /// <summary>
    /// Настройки
    /// </summary>
    public MqttConfigModel? Conf { get; set; }
}