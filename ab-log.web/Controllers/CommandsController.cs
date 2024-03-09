////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Mvc;
using SharedLib;

namespace ABLogWeb;

/// <summary>
/// Команды
/// </summary>
[Route("/api/[controller]"), ApiController]
public class CommandsController(ICommandsService commands) : ControllerBase
{
    /// <summary>
    /// 
    /// </summary>
    [HttpGet($"/{GlobalStatic.Routes.Commands}/{GlobalStatic.Routes.BY_OWNER}/{{script_id}}")]
    public async Task<EntriesSortingResponseModel> GetCommandsEntriesByScript([FromRoute] int script_id, CancellationToken cancellation_token = default)
        => await commands.GetCommandsEntriesByScript(script_id, cancellation_token);

    /// <summary>
    /// 
    /// </summary>
    [HttpGet($"/{GlobalStatic.Routes.Commands}/{GlobalStatic.Routes.GET}/{{command_id}}")]
    public async Task<CommandResponseModel> CommandGet([FromRoute] int command_id, CancellationToken cancellation_token = default)
        => await commands.CommandGet(command_id, cancellation_token);

    /// <summary>
    /// 
    /// </summary>
    [HttpPost($"/{GlobalStatic.Routes.Commands}/{GlobalStatic.Routes.UPDATE}")]
    public async Task<ResponseBaseModel> CommandUpdateOrCreate(CommandModelDB command_json, CancellationToken cancellation_token = default)
        => await commands.CommandUpdateOrCreate(command_json, cancellation_token);

    /// <summary>
    /// 
    /// </summary>
    [HttpPut($"/{GlobalStatic.Routes.Commands}/{GlobalStatic.Routes.SORTING}")]
    public async Task<EntriesSortingResponseModel> CommandSortingSet(IdsPairModel req, CancellationToken cancellation_token = default)
        => await commands.CommandSortingSet(req, cancellation_token);

    /// <summary>
    /// 
    /// </summary>
    [HttpDelete($"/{GlobalStatic.Routes.Commands}/{GlobalStatic.Routes.DELETE}/{{id_command}}")]
    public async Task<ResponseBaseModel> CommandDelete([FromRoute] int id_command, CancellationToken cancellation_token = default)
        => await commands.CommandDelete(id_command, cancellation_token);
}