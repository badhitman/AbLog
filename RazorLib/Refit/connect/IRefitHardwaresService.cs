using SharedLib;
using Refit;

namespace BlazorLib;

/// <summary>
/// Работа с бэком: Hardwares
/// </summary>
[Headers("Content-Type: application/json")]
public interface IRefitHardwaresService
{
    /// <summary>
    /// Получить все устройства (с портами)
    /// </summary>
    /// <returns>Все устройства (с портами)</returns>
    [Get($"/api/{GlobalStatic.HttpRoutes.Hardwares}/{GlobalStatic.HttpRoutes.LIST}")]
    public Task<ApiResponse<HardwaresResponseModel>> HardwaresGetAll(CancellationToken cancellation_token = default);

    /// <summary>
    /// Получить все устройства (данные в лёгкой форме)
    /// </summary>
    /// <returns>Все устройства (данные в лёгкой форме)</returns>
    [Get($"/api/{GlobalStatic.HttpRoutes.Hardwares}/{GlobalStatic.HttpRoutes.ENTRIES}")]
    public Task<ApiResponse<EntriesResponseModel>> HardwaresGetAllAsEntries(CancellationToken cancellation_token = default);

    /// <summary>
    /// Получить все устройства с портами
    /// </summary>
    [Get($"/api/{GlobalStatic.HttpRoutes.Hardwares}/{GlobalStatic.HttpRoutes.NESTED_ENTRIES}")]
    public Task<ApiResponse<EntriesNestedResponseModel>> HardwaresGetTreeNestedEntries(CancellationToken cancellation_token = default);

    /// <summary>
    /// Получить информацию об устройстве
    /// </summary>
    [Get($"/api/{GlobalStatic.HttpRoutes.Hardwares}/{{hardware_id}}")]
    public Task<ApiResponse<HardwareResponseModel>> HardwareGet(int hardware_id, CancellationToken cancellation_token = default);

    /// <summary>
    /// Удалить устройство
    /// </summary>
    [Delete($"/api/{GlobalStatic.HttpRoutes.Hardwares}/{{hardware_id}}")]
    public Task<ApiResponse<ResponseBaseModel>> HardwareDelete(int hardware_id, CancellationToken cancellation_token = default);

    /// <summary>
    /// Обновить/создать устройство (управляющий блок/контроллер)
    /// </summary>
    [Post($"/api/{GlobalStatic.HttpRoutes.Hardwares}/{GlobalStatic.HttpRoutes.UPDATE}")]
    public Task<ApiResponse<HardwareResponseModel>> HardwareUpdate(HardwareBaseModel hardware, CancellationToken cancellation_token = default);

    /// <summary>
    /// Получить порт устройства
    /// </summary>
    [Get($"/api/{GlobalStatic.HttpRoutes.Hardwares}/{GlobalStatic.HttpRoutes.Ports}/{{port_id}}")]
    public Task<ApiResponse<PortHardwareResponseModel>> HardwarePortGet(int port_id, CancellationToken cancellation_token = default);

    /// <summary>
    /// Получить HTML основной страницы устройства
    /// </summary>
    [Post($"/api/{GlobalStatic.HttpRoutes.Hardwares}/{GlobalStatic.HttpRoutes.HTML}/{GlobalStatic.HttpRoutes.MAIN}")]
    public Task<ApiResponse<HttpResponseModel>> GetHardwareHtmlPage(HardvareGetRequestModel req, CancellationToken cancellation_token = default);

    /// <summary>
    /// Запрос/проверка порта устройства
    /// </summary>
    [Post($"/api/{GlobalStatic.HttpRoutes.Hardwares}/{GlobalStatic.HttpRoutes.Ports}/{GlobalStatic.HttpRoutes.CHECK}")]
    public Task<ApiResponse<EntriyResponseModel>> CheckPortHardware(PortHardwareCheckRequestModel req, CancellationToken cancellation_token = default);

    /// <summary>
    /// Установить имя порта
    /// </summary>
    [Post($"/api/{GlobalStatic.HttpRoutes.Hardwares}/{GlobalStatic.HttpRoutes.Ports}/{GlobalStatic.HttpRoutes.UPDATE}")]
    public Task<ApiResponse<ResponseBaseModel>> SetNamePort(EntryModel port_id_name, CancellationToken cancellation_token = default);
}