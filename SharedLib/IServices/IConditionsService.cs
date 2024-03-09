////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Ограничения
/// </summary>
public interface IConditionsService
{
    /// <summary>
    /// Conditions get by owner
    /// </summary>
    public Task<ConditionsAnonimResponseModel> ConditionsGetByOwner(int owner_id, ConditionsTypesEnum condition_type, CancellationToken cancellation_token = default);

    /// <summary>
    /// Condition update or create
    /// </summary>
    public Task<ConditionsAnonimResponseModel> ConditionUpdateOrCreate(ConditionUpdateModel condition_request, CancellationToken cancellation_token = default);

    /// <summary>
    /// Condition delete
    /// </summary>
    public Task<ConditionsAnonimResponseModel> ConditionDelete(int condition_id, ConditionsTypesEnum condition_type, CancellationToken cancellation_token = default);
}