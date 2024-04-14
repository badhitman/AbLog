////////////////////////////////////////////////
// � https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Mvc;
using SharedLib;

namespace ABLogWeb;

/// <summary>
/// ������� (���������).
/// </summary>
[Route("/api/[controller]"), ApiController]
public class SystemController(ISystemCommandsService com_service) : ControllerBase
{
    /// <summary>
    /// ������� (���)
    /// </summary>
    [HttpGet($"{GlobalStatic.Routes.System}-{GlobalStatic.Routes.Commands}/{GlobalStatic.Routes.LIST}")]
    public async Task<SystemCommandsResponseModel> CommandsGetAll(CancellationToken cancellation_token = default)
        => await com_service.CommandsGetAll(cancellation_token);

    /// <summary>
    /// ������� ��������� �������
    /// </summary>
    [HttpDelete($"{GlobalStatic.Routes.System}-{GlobalStatic.Routes.Commands}/{GlobalStatic.Routes.DELETE}/{{comm_id}}")]
    public async Task<ResponseBaseModel> CommandDelete(int comm_id, CancellationToken cancellation_token = default)
        => await com_service.CommandDelete(comm_id, cancellation_token);

    /// <summary>
    /// ���������/��������� ��������� �������
    /// </summary>
    [HttpPut($"{GlobalStatic.Routes.System}-{GlobalStatic.Routes.Commands}/{{comm_id}}")]
    public async Task<ResponseBaseModel> CommandRun(int comm_id, CancellationToken cancellation_token = default)
        => await com_service.CommandRun(comm_id, cancellation_token);

    /// <summary>
    /// ��������/������� ��������� �������
    /// </summary>
    [HttpPost($"{GlobalStatic.Routes.System}-{GlobalStatic.Routes.Commands}/{GlobalStatic.Routes.UPDATE}")]
    public async Task<ResponseBaseModel> CommadndRun(SystemCommandModelDB comm, CancellationToken cancellation_token = default)
        => await com_service.CommandUpdateOrCreate(comm, cancellation_token);
}