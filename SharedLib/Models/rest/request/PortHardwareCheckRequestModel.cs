namespace SharedLib
{
    /// <summary>
    /// Запрос/проверка порта устройства
    /// </summary>
    public class PortHardwareCheckRequestModel
    {
        /// <summary>
        /// Hardware Id
        /// </summary>
        public int HardwareId { get; set; }

        /// <summary>
        /// Номер порта
        /// </summary>
        public int PortNum { get; set; }

        /// <summary>
        /// Создать порт в базе данных (если отсуствует)
        /// </summary>
        public bool CreatePortIfNoptExist { get; set; } = true;
    }
}
