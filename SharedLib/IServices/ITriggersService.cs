////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Триггеры событий
/// </summary>
public interface ITriggersService
{
    /// <summary>
    /// Triggers get all
    /// </summary>
    public Task<TResponseModel<List<TrigerModelDB>>> TriggersGetAll(CancellationToken cancellationToken = default);

    /// <summary>
    /// Trigger update or create
    /// </summary>
    public Task<TResponseModel<List<TrigerModelDB>>> TriggerUpdateOrCreate(TrigerModelDB triggerJson, CancellationToken cancellationToken = default);

    /// <summary>
    /// Trigger delete
    /// </summary>
    public Task<TResponseModel<List<TrigerModelDB>>> TriggerDelete(int triggerId, CancellationToken cancellationToken = default);
}