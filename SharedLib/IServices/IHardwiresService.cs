////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Устройства
/// </summary>
public interface IHardwiresService
{
    /// <summary>
    /// Устройства (все)
    /// </summary>
    public Task<TResponseModel<List<HardwareModel>>> HardwiresGetAll(CancellationToken cancellation_token = default);

    /// <summary>
    /// Устройства (все) в виде Entries[]
    /// </summary>
    public Task<TResponseModel<List<EntryModel>>> HardwiresGetAllAsEntries(CancellationToken cancellation_token = default);

    /// <summary>
    /// Устройства (все) в виде Entries[] вместе с портами
    /// </summary>
    public Task<TResponseModel<List<EntryNestedModel>>> HardwiresGetTreeNestedEntries(CancellationToken cancellation_token = default);

    /// <summary>
    /// Устройство (управляющий блок/контроллер)
    /// </summary>
    public Task<TResponseModel<HardwareBaseModel>> HardwareGet(int hardware_id, CancellationToken cancellation_token = default);

    /// <summary>
    /// Удалить устройство
    /// </summary>
    public Task<ResponseBaseModel> HardwareDelete(int hardware_id, CancellationToken cancellation_token = default);

    /// <summary>
    /// Обновить/создать устройство (управляющий блок/контроллер)
    /// </summary>
    public Task<TResponseModel<HardwareBaseModel>> HardwareUpdate(HardwareBaseModel hardware, CancellationToken cancellation_token = default);

    /// <summary>
    /// Порт контроллера
    /// </summary>
    public Task<TResponseModel<PortHardwareModel>> HardwarePortGet(int port_id, CancellationToken cancellation_token = default);

    /// <summary>
    /// Получить HTML страницы управляющего блока (прим: http://192.168.0.14/sec)
    /// </summary>
    public Task<HttpResponseModel> GetHardwareHtmlPage(HardwareGetHttpRequestModel req, CancellationToken cancellation_token = default);

    /// <summary>
    /// Запрос/проверка порта устройства
    /// </summary>
    /// <returns>ID порта (DB) и его имя</returns>
    public Task<TResponseModel<EntryModel>> CheckPortHardware(PortHardwareCheckRequestModel req, CancellationToken cancellation_token = default);

    /// <summary>
    /// Установить имя порта
    /// </summary>
    public Task<ResponseBaseModel> SetNamePort(EntryModel port_id_name, CancellationToken cancellation_token  = default);
}