using SharedLib;
using Refit;

namespace BlazorLib;

/// <summary>
/// Работа с бэком: Hardwares
/// </summary>
[Headers("Content-Type: application/json")]
public interface IRefitContentionsService
{
    /// <summary>
    /// Получить идентификаторы скриптов, которым запрещён автозапуск во время выполенния  конкурирующего скрипта script_id (ScriptMasterId)
    /// </summary>
    /// <param name="script_id">script_id/ScriptMasterId</param>
    /// <returns>Перечень Id скриптов, которым запрещён автозапуск во время выполенния конкурирующего скрипта script_id (ScriptMasterId)</returns>
    [Get($"/api/{GlobalStatic.HttpRoutes.Contentions}/{{script_id}}")]
    public Task<ApiResponse<IdsResponseModel>> ContentionsGetByScript(int script_id);

    /// <summary>
    /// Установить признак конкурирующей взаимоблокировки
    /// </summary>
    /// <param name="contention_upd">Запрос</param>
    /// <returns>Перечень Id скриптов, которым запрещён автозапуск во время выполенния конкурирующего скрипта ScriptMasterId</returns>
    [Post($"/api/{GlobalStatic.HttpRoutes.Contentions}/{GlobalStatic.HttpRoutes.UPDATE}")]
    public Task<ApiResponse<IdsResponseModel>> ContentionSet(UpdateContentionRequestModel contention_upd);
}