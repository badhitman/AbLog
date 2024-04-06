////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Настройки MQTT
/// </summary>
public class MqttConfigResponseModel : ResponseBaseModel
{
    /// <summary>
    /// Настройки MQTT
    /// </summary>
    public MqttConfigModel? Conf { get; set; }
}