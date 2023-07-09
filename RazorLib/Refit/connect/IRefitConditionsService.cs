////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using SharedLib;
using Refit;

namespace BlazorLib;

/// <summary>
/// Работа с бэком: Conditions
/// </summary>
[Headers("Content-Type: application/json")]
public interface IRefitConditionsService
{
    /// <summary>
    /// Получить условия/ограничения по владельцу (тригшер или команда)
    /// </summary>
    /// <returns>Объект ограничений/условий</returns>
    [Get($"/api/{GlobalStatic.Routes.Conditions}/{{owner_id}}/{{condition_type}}")]
    public Task<ApiResponse<ConditionsAnonimResponseModel>> ConditionsGetByOwner([AliasAs("owner_id")] int condition_id, [AliasAs("condition_type")] ConditionsTypesEnum condition_type);

    /// <summary>
    /// Обновить объект условия/ограничения в БД
    /// </summary>
    /// <param name="req">Объект обновления условия/ограничения</param>
    /// <returns>Полный (обновлённый) перечень условий контекстного типа (например для тригера, команды и т.п.)</returns>
    [Post($"/api/{GlobalStatic.Routes.Conditions}/{GlobalStatic.Routes.UPDATE}")]
    public Task<ApiResponse<ConditionsAnonimResponseModel>> ConditionUpdateOrCreate(UpdateConditionRequestModel req);

    /// <summary>
    /// Удалить объект условия/ограничения
    /// </summary>
    /// <returns>Полный (обновлённый) перечень условий (после удаления) контекстного типа (например для тригера, команды и т.п.)</returns>
    [Delete($"/api/{GlobalStatic.Routes.Conditions}/{{condition_id}}/{{condition_type}}")]
    public Task<ApiResponse<ConditionsAnonimResponseModel>> ConditionDelete([AliasAs("condition_id")] int condition_id, [AliasAs("condition_type")] ConditionsTypesEnum condition_type);
}