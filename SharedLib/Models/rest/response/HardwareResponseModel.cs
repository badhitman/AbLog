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
        public HardwareBaseModel? Hardware { get; set; }
    }
}