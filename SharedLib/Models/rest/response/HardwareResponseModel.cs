namespace SharedLib
{
    /// <summary>
    /// Устройство: Ответ rest/api
    /// </summary>
    public class HardwareResponseModel : ResponseBaseModel
    {
        /// <summary>
        /// Устройство
        /// </summary>
        public HardwareModel? Hardware { get; set; }
    }
}