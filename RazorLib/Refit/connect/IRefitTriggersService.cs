using SharedLib;
using Refit;

namespace BlazorLib;

/// <summary>
/// Работа с бэком: Triggers
/// </summary>
[Headers("Content-Type: application/json")]
public interface IRefitTriggersService
{
    /// <summary>
    /// Получить все тригеры
    /// </summary>
    [Get($"/api/{GlobalStatic.Routes.Triggers}/{GlobalStatic.Routes.LIST}")]
    public Task<ApiResponse<TriggersResponseModel>> TriggersGetAll();

    /// <summary>
    /// Обновить тригер
    /// </summary>
    /// <param name="trigger">Тригер обновления</param>
    /// <returns>Все тригеры</returns>
    [Post($"/api/{GlobalStatic.Routes.Triggers}/{GlobalStatic.Routes.UPDATE}")]
    public Task<ApiResponse<TriggersResponseModel>> TriggerUpdateOrCreate(TrigerModelDB trigger);

    /// <summary>
    /// Удалить тригер
    /// </summary>
    /// <param name="trigger_id">Идентификатор тригшера для удаления</param>
    /// <returns>Все тригеры</returns>
    [Delete($"/api/{GlobalStatic.Routes.Triggers}/{{trigger_id}}")]
    public Task<ApiResponse<TriggersResponseModel>> TriggerDelete(int trigger_id);
}