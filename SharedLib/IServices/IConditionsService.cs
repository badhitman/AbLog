////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Ограничения (для команд, триггеров ...)
/// </summary>
public interface IConditionsService
{
    /// <summary>
    /// Conditions get by owner
    /// </summary>
    public Task<TResponseModel<List<ConditionAnonymModel>>> ConditionsGetByOwner(int owner_id, ConditionsTypesEnum condition_type, CancellationToken cancellation_token = default);

    /// <summary>
    /// Condition update or create
    /// </summary>
    public Task<TResponseModel<List<ConditionAnonymModel>>> ConditionUpdateOrCreate(ConditionUpdateModel condition_request, CancellationToken cancellation_token = default);

    /// <summary>
    /// Condition delete
    /// </summary>
    public Task<TResponseModel<List<ConditionAnonymModel>>> ConditionDelete(int condition_id, ConditionsTypesEnum condition_type, CancellationToken cancellation_token = default);
}