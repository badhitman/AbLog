////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Системные команды
/// </summary>
public interface ISystemCommandsService
{
    /// <summary>
    /// Команды (все)
    /// </summary>
    public Task<SystemCommandsResponseModel> CommandsGetAll(CancellationToken cancellation_token = default);

    /// <summary>
    /// Обновить команду
    /// </summary>
    public Task<ResponseBaseModel> CommandUpdateOrCreate(SystemCommandModelDB comm, CancellationToken cancellation_token = default);

    /// <summary>
    /// Удалить команду
    /// </summary>
    public Task<ResponseBaseModel> CommandDelete(int comm_id, CancellationToken cancellation_token = default);

    /// <summary>
    /// Запустить команду
    /// </summary>
    public Task<ResponseBaseModel> CommandRun(int comm_id, CancellationToken cancellation_token = default);
}