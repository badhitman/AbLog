namespace SharedLib
{
    /// <summary>
    /// Настройки MQTT: Ответ rest/api
    /// </summary>
    public class MqttConfigResponseModel : ResponseBaseModel
    {
        /// <summary>
        /// Порт устройства
        /// </summary>
        public MqttConfigModel? Conf { get; set; }
    }
}