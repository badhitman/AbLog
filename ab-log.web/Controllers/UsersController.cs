////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Mvc;
using SharedLib;

namespace ABLogWeb;

/// <summary>
/// Пользователи
/// </summary>
[Route("/api/[controller]"), ApiController]
public class UsersController(IUsersService users_service) : ControllerBase
{
    /// <summary>
    /// Пользователи
    /// </summary>
    [HttpGet($"/{GlobalStatic.Routes.Users}/{GlobalStatic.Routes.LIST}")]
    public async Task<UsersPaginationResponseModel> UsersGetList([FromQuery] UserListGetModel req, CancellationToken cancellation_token = default)
        => await users_service.UsersGetList(req, cancellation_token);

    /// <summary>
    /// Пользователь
    /// </summary>
    [HttpGet($"/{GlobalStatic.Routes.Users}/{{telegram_id}}")]
    public async Task<UserResponseModel> UserGet([FromRoute] long telegram_id, CancellationToken cancellation_token = default)
        => await users_service.GetUser(telegram_id, cancellation_token);

    /// <summary>
    /// Обновить параметры пользователя
    /// </summary>
    [HttpPut($"/{GlobalStatic.Routes.Users}/{GlobalStatic.Routes.UPDATE}")]
    public async Task<ResponseBaseModel> UserUpdate(UpdateUserModel req, CancellationToken cancellation_token = default)
        => await users_service.UpdateUser(req, cancellation_token);
}