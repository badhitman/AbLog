namespace SharedLib.IServices
{
    /// <summary>
    /// Устройства
    /// </summary>
    public interface IHardwaresService
    {
        /// <summary>
        /// Устройства (все)
        /// </summary>
        public Task<HardwaresResponseModel> HardwaresGetAll();

        /// <summary>
        /// Устройства (все) в виде Entries[]
        /// </summary>
        public Task<EntriesResponseModel> HardwaresGetAllAsEntries();

        /// <summary>
        /// Устройства (все) в виде Entries[] вместе с портами
        /// </summary>
        public Task<EntriesNestedResponseModel> HardwaresGetTreeNestedEntries();

        /// <summary>
        /// Устройство (управляющий блок/контроллер)
        /// </summary>
        public Task<HardwareResponseModel> HardwareGet(int hardware_id);

        /// <summary>
        /// Порт контроллера
        /// </summary>
        public Task<PortHardwareResponseModel> HardwarePortGet(int port_id);
    }
}