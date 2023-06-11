namespace SharedLib
{
    /// <summary>
    /// Перечень устройств: Ответ rest/api
    /// </summary>
    public class HardwaresResponseModel : ResponseBaseModel
    {
        /// <summary>
        /// Перечень устройств
        /// </summary>
        public IEnumerable<HardwareModel>? Hardwares { get; set; }
    }
}