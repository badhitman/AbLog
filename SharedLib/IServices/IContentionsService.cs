////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Конкуренции команд скриптов
/// </summary>
public interface IContentionsService
{
    /// <summary>
    /// Получить настройки конкуренции команд скриптов
    /// </summary>
    public Task<IdsResponseModel> ContentionsGetByScript(int script_id, CancellationToken cancellation_token = default);

    /// <summary>
    /// Установить правила конкуренции выполнения команд скриптов
    /// </summary>
    public Task<IdsResponseModel> ContentionSet(ContentionUpdateModel contention_json, CancellationToken cancellation_token = default);
}