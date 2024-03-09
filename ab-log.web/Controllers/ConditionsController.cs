////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Mvc;
using SharedLib;

namespace ABLogWeb;

/// <summary>
/// Ограничения
/// </summary>
[Route("/api/[controller]"), ApiController]
public class ConditionsController(IConditionsService Conditions) : ControllerBase
{
    /// <summary>
    /// 
    /// </summary>
    [HttpGet($"/{GlobalStatic.Routes.Conditions}/{GlobalStatic.Routes.BY_OWNER}/{{owner_id}}/{{condition_type}}")]
    public async Task<ConditionsAnonimResponseModel> ConditionsGetByOwner([FromRoute] int owner_id, [FromRoute] ConditionsTypesEnum condition_type, CancellationToken cancellation_token = default)
        => await Conditions.ConditionsGetByOwner(owner_id, condition_type, cancellation_token);

    /// <summary>
    /// 
    /// </summary>
    [HttpPost($"/{GlobalStatic.Routes.Conditions}/{GlobalStatic.Routes.UPDATE}")]
    public async Task<ConditionsAnonimResponseModel> ConditionUpdateOrCreate(ConditionUpdateModel condition_request, CancellationToken cancellation_token = default)
        => await Conditions.ConditionUpdateOrCreate(condition_request, cancellation_token);

    /// <summary>
    /// 
    /// </summary>
    [HttpDelete($"/{GlobalStatic.Routes.Conditions}/{{condition_id}}/{{condition_type}}")]
    public async Task<ConditionsAnonimResponseModel> ConditionDelete([FromRoute] int condition_id, [FromRoute] ConditionsTypesEnum condition_type, CancellationToken cancellation_token = default)
        => await Conditions.ConditionDelete(condition_id, condition_type, cancellation_token);
}