////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Mvc;
using SharedLib;

namespace ABLogWeb;

/// <summary>
/// Scripts
/// </summary>
[Route("/api/[controller]"), ApiController]
public class ScriptsController(IScriptsService Scripts) : ControllerBase
{
    /// <summary>
    /// Scripts: Get All
    /// </summary>
    [HttpGet($"/{GlobalStatic.Routes.Scripts}/{GlobalStatic.Routes.LIST}")]
    public async Task<TResponseModel<List<ScriptModelDB>>> ScriptsGetAll(CancellationToken cancellation_token = default)
        => await Scripts.ScriptsGetAll(cancellation_token);

    /// <summary>
    /// Script delete
    /// </summary>
    [HttpDelete($"/{GlobalStatic.Routes.Scripts}/{{script_id}}")]
    public async Task<TResponseModel<List<ScriptModelDB>>> ScriptDelete([FromRoute] int script_id, CancellationToken cancellation_token = default)
        => await Scripts.ScriptDelete(script_id, cancellation_token);

    /// <summary>
    /// Script update or create
    /// </summary>
    [HttpPost($"/{GlobalStatic.Routes.Scripts}/{GlobalStatic.Routes.UPDATE}")]
    public async Task<TResponseModel<List<ScriptModelDB>>> ScriptUpdateOrCreate(EntryDescriptionModel script, CancellationToken cancellation_token = default)
        => await Scripts.ScriptUpdateOrCreate(script, cancellation_token);

    /// <summary>
    /// Script enable or disable
    /// </summary>
    [HttpPut($"/{GlobalStatic.Routes.Scripts}/{GlobalStatic.Routes.ENABLE}")]
    public async Task<ResponseBaseModel> ScriptEnableSet(ObjectStateModel req, CancellationToken cancellation_token = default)
        => await Scripts.ScriptEnableSet(req, cancellation_token);
}