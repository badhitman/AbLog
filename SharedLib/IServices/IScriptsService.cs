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
    public Task<TResponseModel<List<ScriptModelDB>>> ScriptsGetAll(CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить скрипт
    /// </summary>
    public Task<TResponseModel<List<ScriptModelDB>>> ScriptDelete(int script_id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновить/Создать скрипт
    /// </summary>
    public Task<TResponseModel<List<ScriptModelDB>>> ScriptUpdateOrCreate(EntryDescriptionModel script, CancellationToken cancellationToken = default);

    /// <summary>
    /// Включение/Выключение скрипта
    /// </summary>
    public Task<ResponseBaseModel> ScriptEnableSet(ObjectStateModel req, CancellationToken cancellationToken = default);
}
