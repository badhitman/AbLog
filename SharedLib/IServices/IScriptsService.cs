////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Скрипты
/// </summary>
public interface IScriptsService
{
    /// <summary>
    /// Все скрипты
    /// </summary>
    public Task<ScriptsResponseModel> ScriptsGetAll(CancellationToken cancellation_token = default);

    /// <summary>
    /// Удалить скрипт
    /// </summary>
    public Task<ScriptsResponseModel> ScriptDelete(int script_id, CancellationToken cancellation_token = default);

    /// <summary>
    /// Обновить/Создать скрипт
    /// </summary>
    public Task<ScriptsResponseModel> ScriptUpdateOrCreate(EntryDescriptionModel script, CancellationToken cancellation_token = default);

    /// <summary>
    /// Включение/Выключение скрипта
    /// </summary>
    public Task<ResponseBaseModel> ScriptEnableSet(ObjectStateModel req, CancellationToken cancellation_token = default);
}
