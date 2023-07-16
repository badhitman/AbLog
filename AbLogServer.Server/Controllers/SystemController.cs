////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Mvc;
using SharedLib;

namespace ABLog;

/// <summary>
/// Команды (системные)
/// </summary>
[ApiController]
[Route("/api/[controller]")]
public class SystemController : ControllerBase
{
    readonly ILogger<SystemController> _logger;
    readonly ISystemCommandsService _com_service;

    /// <summary>
    /// Команды (системные)
    /// </summary>
    public SystemController(ILogger<SystemController> logger, ISystemCommandsService com_service)
    {
        _logger = logger;
        _com_service = com_service;
    }

    /// <summary>
    /// Команды (все)
    /// </summary>
    [HttpGet($"{GlobalStatic.Routes.Commands}/{GlobalStatic.Routes.LIST}")]
    public async Task<SystemCommandsResponseModel> CommandsGetAll() => await _com_service.CommandsGetAll();

    /// <summary>
    /// 
    /// </summary>
    [HttpDelete($"{GlobalStatic.Routes.Commands}/{GlobalStatic.Routes.DELETE}/{{comm_id}}")]
    public async Task<ResponseBaseModel> CommandDelete(int comm_id) => await _com_service.CommandDelete(comm_id);

    /// <summary>
    /// 
    /// </summary>
    [HttpPut($"{GlobalStatic.Routes.Commands}/{{comm_id}}")]
    public async Task<ResponseBaseModel> CommandRun(int comm_id) => await _com_service.CommandRun(comm_id);

    /// <summary>
    /// 
    /// </summary>
    [HttpPost($"{GlobalStatic.Routes.Commands}/{GlobalStatic.Routes.UPDATE}")]
    public async Task<ResponseBaseModel> CommadndRun(SystemCommandModelDB comm) => await _com_service.CommandUpdateOrCreate(comm);
}