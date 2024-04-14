////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Mvc;
using SharedLib;

namespace ABLogWeb;

/// <summary>
/// Команды (системные).
/// </summary>
[Route("/api/[controller]"), ApiController]
public class SystemController(ISystemCommandsService com_service) : ControllerBase
{
    /// <summary>
    /// Команды (все)
    /// </summary>
    [HttpGet($"{GlobalStatic.Routes.System}-{GlobalStatic.Routes.Commands}/{GlobalStatic.Routes.LIST}")]
    public async Task<SystemCommandsResponseModel> CommandsGetAll(CancellationToken cancellation_token = default)
        => await com_service.CommandsGetAll(cancellation_token);

    /// <summary>
    /// Удалить системную команду
    /// </summary>
    [HttpDelete($"{GlobalStatic.Routes.System}-{GlobalStatic.Routes.Commands}/{GlobalStatic.Routes.DELETE}/{{comm_id}}")]
    public async Task<ResponseBaseModel> CommandDelete(int comm_id, CancellationToken cancellation_token = default)
        => await com_service.CommandDelete(comm_id, cancellation_token);

    /// <summary>
    /// Запустить/выполнить системную команду
    /// </summary>
    [HttpPut($"{GlobalStatic.Routes.System}-{GlobalStatic.Routes.Commands}/{{comm_id}}")]
    public async Task<ResponseBaseModel> CommandRun(int comm_id, CancellationToken cancellation_token = default)
        => await com_service.CommandRun(comm_id, cancellation_token);

    /// <summary>
    /// Обновить/создать системную команду
    /// </summary>
    [HttpPost($"{GlobalStatic.Routes.System}-{GlobalStatic.Routes.Commands}/{GlobalStatic.Routes.UPDATE}")]
    public async Task<ResponseBaseModel> CommadndRun(SystemCommandModelDB comm, CancellationToken cancellation_token = default)
        => await com_service.CommandUpdateOrCreate(comm, cancellation_token);
}