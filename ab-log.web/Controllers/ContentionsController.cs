////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Mvc;
using SharedLib;

namespace ABLogWeb;

/// <summary>
/// Конкуренция
/// </summary>
[Route("/api/[controller]"), ApiController]
public class ContentionsController(IContentionsService contentions) : ControllerBase
{
    /// <summary>
    /// Получить настройки конкуренции команд скрипта
    /// </summary>
    [HttpGet($"/{GlobalStatic.Routes.Contentions}/{GlobalStatic.Routes.BY_OWNER}-{GlobalStatic.Routes.Scripts}/{{script_id}}")]
    public async Task<TResponseModel<List<int>>> ContentionsGetByScript([FromRoute] int script_id, CancellationToken cancellation_token = default)
        => await contentions.ContentionsGetByScript(script_id, cancellation_token);

    /// <summary>
    /// Установить правила конкуренции выполнения команд скриптов
    /// </summary>
    [HttpPost($"/{GlobalStatic.Routes.Contentions}/{GlobalStatic.Routes.UPDATE}")]
    public async Task<TResponseModel<List<int>>> ContentionSet(ContentionUpdateModel contention_json, CancellationToken cancellation_token = default)
        => await contentions.ContentionSet(contention_json, cancellation_token);
}