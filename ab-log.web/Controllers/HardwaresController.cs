////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Mvc;
using SharedLib;

namespace ABLogWeb;

/// <summary>
/// Управляющие блоки
/// </summary>
[Route("/api/[controller]"), ApiController]
public class HardwaresController(IHardwaresService hardwares_service) : ControllerBase
{
    /// <summary>
    /// Устройства (все)
    /// </summary>
    [HttpGet($"/{GlobalStatic.Routes.Hardwares}/{GlobalStatic.Routes.LIST}")]
    public async Task<HardwaresResponseModel> HardwaresGetAll(CancellationToken cancellation_token = default)
        => await hardwares_service.HardwaresGetAll(cancellation_token);

    /// <summary>
    /// Устройства (все) в виде Entries[]
    /// </summary>
    [HttpGet($"/{GlobalStatic.Routes.Hardwares}/{GlobalStatic.Routes.ENTRIES}")]
    public async Task<EntriesResponseModel> HardwaresGetAllAsEntries(CancellationToken cancellation_token = default)
        => await hardwares_service.HardwaresGetAllAsEntries(cancellation_token);

    /// <summary>
    /// Устройства (все) в виде Entries[] вместе с портами
    /// </summary>
    [HttpGet($"/{GlobalStatic.Routes.Hardwares}/{GlobalStatic.Routes.NESTED_ENTRIES}")]
    public async Task<EntriesNestedResponseModel> HardwaresGetTreeNestedEntries(CancellationToken cancellation_token = default)
        => await hardwares_service.HardwaresGetTreeNestedEntries(cancellation_token);

    /// <summary>
    /// Устройство (управляющий блок/контроллер)
    /// </summary>
    [HttpGet($"/{GlobalStatic.Routes.Hardwares}/{GlobalStatic.Routes.GET}/{{hardware_id}}")]
    public async Task<HardwareResponseModel> HardwareGet([FromRoute] int hardware_id, CancellationToken cancellation_token = default)
        => await hardwares_service.HardwareGet(hardware_id, cancellation_token);

    /// <summary>
    /// Удалить устройство
    /// </summary>
    [HttpDelete($"/{GlobalStatic.Routes.Hardwares}/{GlobalStatic.Routes.DELETE}/{{hardware_id}}")]
    public async Task<ResponseBaseModel> HardwareDelete([FromRoute] int hardware_id, CancellationToken cancellation_token = default)
        => await hardwares_service.HardwareDelete(hardware_id, cancellation_token);

    /// <summary>
    /// Обновить/создать устройство (управляющий блок/контроллер)
    /// </summary>
    [HttpPost($"/{GlobalStatic.Routes.Hardwares}/{GlobalStatic.Routes.UPDATE}")]
    public async Task<HardwareResponseModel> HardwareUpdate(HardwareBaseModel hardware, CancellationToken cancellation_token = default)
        => await hardwares_service.HardwareUpdate(hardware, cancellation_token);

    /// <summary>
    /// Порт контроллера
    /// </summary>
    [HttpGet($"/{GlobalStatic.Routes.Ports}/{{port_id}}")]
    public async Task<PortHardwareResponseModel> HardwarePortGet([FromRoute] int port_id, CancellationToken cancellation_token = default)
        => await hardwares_service.HardwarePortGet(port_id, cancellation_token);

    /// <summary>
    /// Получить HTML основной страницы устройства
    /// </summary>
    [HttpPost($"/{GlobalStatic.Routes.Hardwares}/{GlobalStatic.Routes.MAIN}-{GlobalStatic.Routes.HTML}")]
    public async Task<HttpResponseModel> GetHardwareHtmlPage(HardwareGetHttpRequestModel req, CancellationToken cancellation_token = default)
        => await hardwares_service.GetHardwareHtmlPage(req, cancellation_token);

    /// <summary>
    /// Запрос/проверка порта устройства
    /// </summary>
    [HttpPost($"/{GlobalStatic.Routes.Ports}/{GlobalStatic.Routes.CHECK}")]
    public async Task<EntriyResponseModel> CheckPortHardware(PortHardwareCheckRequestModel req, CancellationToken cancellation_token = default)
        => await hardwares_service.CheckPortHardware(req, cancellation_token);

    /// <summary>
    /// Установить имя порта
    /// </summary>
    [HttpPost($"{GlobalStatic.Routes.Ports}/{GlobalStatic.Routes.UPDATE}")]
    public async Task<ResponseBaseModel> SetNamePort(EntryModel port_id_name, CancellationToken cancellation_token = default)
        => await hardwares_service.SetNamePort(port_id_name, cancellation_token);
}