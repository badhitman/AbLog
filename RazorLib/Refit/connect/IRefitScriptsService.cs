using SharedLib;
using Refit;

namespace BlazorLib;

/// <summary>
/// Работа с бэком: Scripts
/// </summary>
[Headers("Content-Type: application/json")]
public interface IRefitScriptsService
{
    /// <summary>
    /// Получить все скрипты
    /// </summary>
    /// <returns>Все скрипты</returns>
    [Get($"/api/{GlobalStatic.Routes.Scripts}/{GlobalStatic.Routes.LIST}")]
    public Task<ApiResponse<ScriptsResponseModel>> ScriptsGetAll();

    /// <summary>
    /// Удалить скрипт
    /// </summary>
    /// <param name="script_id">Идентификатор срипта</param>
    /// <returns>Актуальный перечень скриптов (после удаления)</returns>
    [Delete($"/api/{GlobalStatic.Routes.Scripts}/{{script_id}}")]
    public Task<ApiResponse<ScriptsResponseModel>> ScriptDelete(int script_id);

    /// <summary>
    /// Обновить/создать скрипт
    /// </summary>
    /// <param name="script">Скрипт для создания</param>
    /// <returns>Актуальный перечень скриптов (после обновления)</returns>
    [Post($"/api/{GlobalStatic.Routes.Scripts}/{GlobalStatic.Routes.UPDATE}")]
    public Task<ApiResponse<ScriptsResponseModel>> ScriptUpdateOrCreate(EntryDescriptionModel script);

    /// <summary>
    /// Установить скрипту признак включения/отключения
    /// </summary>
    [Put($"/api/{GlobalStatic.Routes.Scripts}/{GlobalStatic.Routes.ENABLE}/{{script_id}}")]
    public Task<ApiResponse<ResponseBaseModel>> ScriptEnableSet([AliasAs("script_id")] int script_id, bool is_enable);
}