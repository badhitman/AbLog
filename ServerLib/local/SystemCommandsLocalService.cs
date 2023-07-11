////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using SharedLib;

namespace ServerLib;

/// <summary>
/// 
/// </summary>
public class SystemCommandsLocalService : ISystemCommandsService
{
    /// <inheritdoc/>
    public Task<ResponseBaseModel> CommandDelete(int comm_id, CancellationToken cancellation_token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<SystemCommandsResponseModel> CommandsGetAll(CancellationToken cancellation_token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<ResponseBaseModel> CommandUpdateOrCreate(SystemCommandModelDB comm, CancellationToken cancellation_token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<ResponseBaseModel> CommandRun(int comm_id, CancellationToken cancellation_token = default)
    {
        throw new NotImplementedException();
    }
}