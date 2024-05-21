////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Mvc;
using SharedLib;

namespace ABLogWeb;

/// <summary>
/// Триггеры событий
/// </summary>
[Route("/api/[controller]"), ApiController]
public class TriggersController(ITriggersService Triggers) : ControllerBase
{
    /// <summary>
    /// Поучит все триггеры
    /// </summary>
    [HttpGet($"/{GlobalStatic.Routes.Triggers}/{GlobalStatic.Routes.LIST}")]
    public async Task<TResponseModel<List<TrigerModelDB>>> TriggersGetAll(CancellationToken cancellation_token = default)
        => await Triggers.TriggersGetAll(cancellation_token);

    /// <summary>
    /// Обновить/создать триггер
    /// </summary>
    [HttpPost($"/{GlobalStatic.Routes.Triggers}/{GlobalStatic.Routes.UPDATE}")]
    public async Task<TResponseModel<List<TrigerModelDB>>> TriggerUpdateOrCreate(TrigerModelDB trigger_json, CancellationToken cancellation_token = default)
        => await Triggers.TriggerUpdateOrCreate(trigger_json, cancellation_token);

    /// <summary>
    /// Удалить триггер
    /// </summary>
    [HttpDelete($"/{GlobalStatic.Routes.Triggers}/{GlobalStatic.Routes.DELETE}/{{trigger_id}}")]
    public async Task<TResponseModel<List<TrigerModelDB>>> TriggerDelete([FromRoute] int trigger_id, CancellationToken cancellation_token = default)
        => await Triggers.TriggerDelete(trigger_id, cancellation_token);
}