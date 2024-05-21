////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Команды скриптов для управляющего контролера
/// </summary>
public interface ICommandsService
{
    /// <summary>
    /// Получить команды скрипта
    /// </summary>
    public Task<TResponseModel<List<EntrySortingModel>>> GetCommandsEntriesByScript(int script_id, CancellationToken cancellation_token = default);

    /// <summary>
    /// Получить команду (по его идентификатору)
    /// </summary>
    public Task<TResponseModel<CommandModelDB>> CommandGet(int command_id, CancellationToken cancellation_token = default);

    /// <summary>
    /// Создать/Обновить команду
    /// </summary>
    public Task<ResponseBaseModel> CommandUpdateOrCreate(CommandModelDB command_json, CancellationToken cancellation_token = default);

    /// <summary>
    /// Установить индексы сортировки команды (упорядочивание)
    /// </summary>
    public Task<TResponseModel<List<EntrySortingModel>>> CommandSortingSet(IdsPairModel req, CancellationToken cancellation_token = default);

    /// <summary>
    /// Удалить команду контролера из скрипта
    /// </summary>
    public Task<ResponseBaseModel> CommandDelete(int id_command, CancellationToken cancellation_token = default);
}