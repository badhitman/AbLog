////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using BlazorLib;
using SharedLib;

namespace RazorLib;

/// <summary>
/// Устройства
/// </summary>
public class SystemCommandsRefitService : ISystemCommandsService
{
    readonly IRefitSystemCommandsService _refit_com;

    /// <summary>
    /// Устройства
    /// </summary>
    public SystemCommandsRefitService(IRefitSystemCommandsService refit_com)
    {
        _refit_com = refit_com;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> CommandDelete(int comm_id, CancellationToken cancellation_token = default)
    {
        Refit.ApiResponse<ResponseBaseModel> rest = await _refit_com.CommandDelete(comm_id, cancellation_token);

        if (rest.Content is null)
            return new HttpResponseModel(ResponseBaseModel.CreateError("rest.Content is null // error {D1D490CB-D2F3-459B-A8C0-88EDD8EBEFDC}"));

        return rest.Content;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> CommandRun(int comm_id, CancellationToken cancellation_token = default)
    {
        Refit.ApiResponse<ResponseBaseModel> rest = await _refit_com.CommandRun(comm_id, cancellation_token);

        if (rest.Content is null)
            return new HttpResponseModel(ResponseBaseModel.CreateError("rest.Content is null // error {A05AFCC9-AA5B-4C07-91AF-26EE0549DB04}"));

        return rest.Content;
    }

    /// <inheritdoc/>
    public async Task<SystemCommandsResponseModel> CommandsGetAll(CancellationToken cancellation_token = default)
    {
        SystemCommandsResponseModel res = new();
        Refit.ApiResponse<SystemCommandsResponseModel> rest = await _refit_com.CommandsGetAll(cancellation_token);

        if (rest.Content is null)
        {
            res.AddError("rest.Content is null // error {CC5769DD-9819-41E3-8C41-9D89175311CE}");
            return res;
        }
        return rest.Content;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> CommandUpdateOrCreate(SystemCommandModelDB comm, CancellationToken cancellation_token = default)
    {
        ResponseBaseModel res = new();
        Refit.ApiResponse<ResponseBaseModel> rest = await _refit_com.CommandUpdateOrCreate(comm, cancellation_token);

        if (rest.Content is null)
        {
            res.AddError("rest.Content is null // error {7189EA09-D15D-4916-A10C-DD4E3D43F03C}");
            return res;
        }
        return rest.Content;
    }
}