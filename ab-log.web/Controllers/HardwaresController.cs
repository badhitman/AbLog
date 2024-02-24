////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Mvc;
using SharedLib;

namespace ABLogWeb;

/// <summary>
/// Управляющие блоки
/// </summary>
[ApiController]
[Route("/api/[controller]")]
public class HardwaresController : ControllerBase
{
    readonly ILogger<HardwaresController> _logger;
    readonly IHardwaresService _hardwares_service;

    /// <summary>
    /// Управляющие блоки
    /// </summary>
    public HardwaresController(ILogger<HardwaresController> logger, IHardwaresService hardwares_service)
    {
        _logger = logger;
        _hardwares_service = hardwares_service;
    }

    /// <summary>
    /// Устройства (все)
    /// </summary>
    [HttpGet($"{GlobalStatic.Routes.LIST}")]
    public async Task<HardwaresResponseModel> HardwaresGetAll() => await _hardwares_service.HardwaresGetAll();

    /// <summary>
    /// Устройства (все) в виде Entries[]
    /// </summary>
    [HttpGet($"{GlobalStatic.Routes.ENTRIES}")]
    public async Task<EntriesResponseModel> HardwaresGetAllAsEntries() => await _hardwares_service.HardwaresGetAllAsEntries();

    /// <summary>
    /// Устройства (все) в виде Entries[] вместе с портами
    /// </summary>
    [HttpGet($"{GlobalStatic.Routes.NESTED_ENTRIES}")]
    public async Task<EntriesNestedResponseModel> HardwaresGetTreeNestedEntries() => await _hardwares_service.HardwaresGetTreeNestedEntries();

    /// <summary>
    /// Устройство (управляющий блок/контроллер)
    /// </summary>
    [HttpGet("{hardware_id}")]
    public async Task<HardwareResponseModel> HardwareGet(int hardware_id) => await _hardwares_service.HardwareGet(hardware_id);

    /// <summary>
    /// Удалить устройство
    /// </summary>
    [HttpDelete("{hardware_id}")]
    public async Task<ResponseBaseModel> HardwareDelete(int hardware_id) => await _hardwares_service.HardwareDelete(hardware_id);

    /// <summary>
    /// Обновить/создать устройство (управляющий блок/контроллер)
    /// </summary>
    [HttpPost($"{GlobalStatic.Routes.UPDATE}")]
    public async Task<HardwareResponseModel> HardwareUpdate(HardwareBaseModel hardware) => await _hardwares_service.HardwareUpdate(hardware);

    /// <summary>
    /// Порт контроллера
    /// </summary>
    [HttpGet($"{GlobalStatic.Routes.Ports}/{{port_id}}")]
    public async Task<PortHardwareResponseModel> HardwarePortGet(int port_id) => await _hardwares_service.HardwarePortGet(port_id);

    /// <summary>
    /// Получить HTML основной страницы устройства
    /// </summary>
    [HttpPost($"{GlobalStatic.Routes.HTML}/{GlobalStatic.Routes.MAIN}")]
    public async Task<HttpResponseModel> GetHardwareHtmlPage(HardwareGetHttpRequestModel req) => await _hardwares_service.GetHardwareHtmlPage(req);

    /// <summary>
    /// Запрос/проверка порта устройства
    /// </summary>
    [HttpPost($"{GlobalStatic.Routes.Ports}/{GlobalStatic.Routes.CHECK}")]
    public async Task<EntriyResponseModel> CheckPortHardware(PortHardwareCheckRequestModel req) => await _hardwares_service.CheckPortHardware(req);

    /// <summary>
    /// Установить имя порта
    /// </summary>
    [HttpPost($"{GlobalStatic.Routes.Ports}/{GlobalStatic.Routes.UPDATE}")]
    public async Task<ResponseBaseModel> SetNamePort(EntryModel port_id_name) => await _hardwares_service.SetNamePort(port_id_name);
}