////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Тригеры событий
/// </summary>
public interface ITriggersService
{
    /// <summary>
    /// Triggers get all
    /// </summary>
    public Task<TriggersResponseModel> TriggersGetAll(CancellationToken cancellation_token = default);

    /// <summary>
    /// Trigger update or create
    /// </summary>
    public Task<TriggersResponseModel> TriggerUpdateOrCreate(TrigerModelDB trigger_json, CancellationToken cancellation_token = default);

    /// <summary>
    /// Trigger delete
    /// </summary>
    public Task<TriggersResponseModel> TriggerDelete(int trigger_id, CancellationToken cancellation_token = default);
}