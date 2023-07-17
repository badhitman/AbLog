////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using BlazorLib;
using SharedLib;

namespace RazorLib;

/// <summary>
/// 
/// </summary>
public class UsersRefitService : IUsersService
{
    readonly IRefitUsersService _refit_user;

    /// <summary>
    /// 
    /// </summary>
    public UsersRefitService(IRefitUsersService refit_user)
    {
        _refit_user = refit_user;
    }

    /// <inheritdoc/>
    public async Task<UserResponseModel> GetUser(long telegram_id, CancellationToken cancellation_token = default)
    {
        UserResponseModel res = new();
        Refit.ApiResponse<UserResponseModel> rest = await _refit_user.UserGet(telegram_id, cancellation_token);

        if (rest.Content is null)
        {
            res.AddError("rest.Content is null // error {C36DADC8-A9EE-438D-AA4E-A0155DC37B16}");
            return res;
        }
        return rest.Content;
    }

    /// <inheritdoc/>
    public async Task<UsersPaginationResponseModel> UsersGetList(UserListGetModel req, CancellationToken cancellation_token = default)
    {
        UsersPaginationResponseModel res = new();
        Refit.ApiResponse<UsersPaginationResponseModel> rest = await _refit_user.UsersGetList(req, cancellation_token);

        if (rest.Content is null)
        {
            res.AddError("rest.Content is null // error {AC8EAA5B-66AF-45B5-9E66-2D8438053296}");
            return res;
        }

        return rest.Content;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateUser(UpdateUserModel req, CancellationToken cancellation_token = default)
    {
        Refit.ApiResponse<ResponseBaseModel> rest = await _refit_user.UserUpdate(req, cancellation_token);

        if (rest.Content is null)
        {
            return ResponseBaseModel.CreateError("rest.Content is null // error {D369BF0C-EFB7-4FD0-8DBE-25EB4485134D}");
        }
        return rest.Content;
    }
}