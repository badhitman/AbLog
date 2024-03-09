////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Конкуренции
/// </summary>
public interface IContentionsService
{
    /// <summary>
    /// 
    /// </summary>
    public Task<IdsResponseModel> ContentionsGetByScript(int script_id, CancellationToken cancellation_token = default);

    /// <summary>
    /// 
    /// </summary>
    public Task<IdsResponseModel> ContentionSet(ContentionUpdateModel contention_json, CancellationToken cancellation_token = default);
}