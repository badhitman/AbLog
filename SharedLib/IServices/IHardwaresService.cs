////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Устройства
/// </summary>
public interface IHardwaresService
{
    /// <summary>
    /// Устройства (все)
    /// </summary>
    public Task<HardwaresResponseModel> HardwaresGetAll(CancellationToken cancellation_token = default);

    /// <summary>
    /// Устройства (все) в виде Entries[]
    /// </summary>
    public Task<EntriesResponseModel> HardwaresGetAllAsEntries(CancellationToken cancellation_token = default);

    /// <summary>
    /// Устройства (все) в виде Entries[] вместе с портами
    /// </summary>
    public Task<EntriesNestedResponseModel> HardwaresGetTreeNestedEntries(CancellationToken cancellation_token = default);

    /// <summary>
    /// Устройство (управляющий блок/контроллер)
    /// </summary>
    public Task<HardwareResponseModel> HardwareGet(int hardware_id, CancellationToken cancellation_token = default);

    /// <summary>
    /// Удалить устройство
    /// </summary>
    public Task<ResponseBaseModel> HardwareDelete(int hardware_id, CancellationToken cancellation_token = default);

    /// <summary>
    /// Обновить/создать устройство (управляющий блок/контроллер)
    /// </summary>
    public Task<HardwareResponseModel> HardwareUpdate(HardwareBaseModel hardware, CancellationToken cancellation_token = default);

    /// <summary>
    /// Порт контроллера
    /// </summary>
    public Task<PortHardwareResponseModel> HardwarePortGet(int port_id, CancellationToken cancellation_token = default);

    /// <summary>
    /// Получить HTML страницы управляющего блока (прим: http://192.168.0.14/sec)
    /// </summary>
    public Task<HttpResponseModel> GetHardwareHtmlPage(HardwareGetHttpRequestModel req, CancellationToken cancellation_token = default);

    /// <summary>
    /// Запрос/проверка порта устройства
    /// </summary>
    /// <returns>ID порта (DB) и его имя</returns>
    public Task<EntriyResponseModel> CheckPortHardware(PortHardwareCheckRequestModel req, CancellationToken cancellation_token = default);

    /// <summary>
    /// Установить имя порта
    /// </summary>
    public Task<ResponseBaseModel> SetNamePort(EntryModel port_id_name, CancellationToken cancellation_token  = default);
}