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
        /// Удалить устройство
        /// </summary>
        public Task<ResponseBaseModel> HardwareDelete(int hardware_id);

        /// <summary>
        /// Обновить/создать устройство (управляющий блок/контроллер)
        /// </summary>
        public Task<HardwareResponseModel> HardwareUpdate(HardwareBaseModel hardware);

        /// <summary>
        /// Порт контроллера
        /// </summary>
        public Task<PortHardwareResponseModel> HardwarePortGet(int port_id);

        /// <summary>
        /// Получить HTML страницы управляющего блока (прим: http://192.168.0.14/sec)
        /// </summary>
        public Task<HttpResponseModel> GetHardwareHtmlPage(HardvareGetRequestModel req);

        /// <summary>
        /// Запрос/проверка порта устройства
        /// </summary>
        /// <returns>ID порта (DB) и его имя</returns>
        public Task<EntriyResponseModel> CheckPortHardware(PortHardwareCheckRequestModel req);

        /// <summary>
        /// Установить имя порта
        /// </summary>
        public Task<ResponseBaseModel> SetNamePort(EntryModel port_id_name);
    }
}