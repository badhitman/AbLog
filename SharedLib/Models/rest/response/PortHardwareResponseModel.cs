namespace SharedLib
{
    /// <summary>
    /// Порт устройства: Ответ rest/api
    /// </summary>
    public class PortHardwareResponseModel : ResponseBaseModel
    {
        /// <summary>
        /// Порт устройства
        /// </summary>
        public PortHardwareModel? Port { get; set; }
    }
}