////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// 
/// </summary>
public interface ICommandsService
{
    /// <summary>
    /// 
    /// </summary>
    public Task<EntriesSortingResponseModel> GetCommandsEntriesByScript(int script_id, CancellationToken cancellation_token = default);

    /// <summary>
    /// 
    /// </summary>
    public Task<CommandResponseModel> CommandGet(int command_id, CancellationToken cancellation_token = default);

    /// <summary>
    /// 
    /// </summary>
    public Task<ResponseBaseModel> CommandUpdateOrCreate(CommandModelDB command_json, CancellationToken cancellation_token = default);

    /// <summary>
    /// 
    /// </summary>
    public Task<EntriesSortingResponseModel> CommandSortingSet(IdsPairModel req, CancellationToken cancellation_token = default);

    /// <summary>
    /// 
    /// </summary>
    public Task<ResponseBaseModel> CommandDelete(int id_command, CancellationToken cancellation_token = default);
}