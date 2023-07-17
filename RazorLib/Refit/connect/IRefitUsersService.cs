////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using SharedLib;
using Refit;

namespace BlazorLib;

/// <summary>
/// Работа с бэком: Users
/// </summary>
[Headers("Content-Type: application/json")]
public interface IRefitUsersService
{
    /// <summary>
    /// Пользователи
    /// </summary>
    [Get($"/api/{GlobalStatic.Routes.Users}/{GlobalStatic.Routes.GET}/{GlobalStatic.Routes.LIST}")]
    public Task<ApiResponse<UsersPaginationResponseModel>> UsersGetList(UserListGetModel req, CancellationToken cancellation_token = default);

    /// <summary>
    /// Пользователи
    /// </summary>
    [Get($"/api/{GlobalStatic.Routes.Users}/{GlobalStatic.Routes.GET}/{{telegram_id}}")]
    public Task<ApiResponse<UserResponseModel>> UserGet(long telegram_id, CancellationToken cancellation_token = default);

    /// <summary>
    /// Пользователи
    /// </summary>
    [Put($"/api/{GlobalStatic.Routes.Users}/{GlobalStatic.Routes.UPDATE}")]
    public Task<ApiResponse<ResponseBaseModel>> UserUpdate(UpdateUserModel req, CancellationToken cancellation_token = default);
}