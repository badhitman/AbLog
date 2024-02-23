////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Mvc;
using SharedLib;

namespace ABLog;

/// <summary>
/// Пользователи
/// </summary>
[ApiController]
[Route("/api/[controller]")]
public class UsersController : ControllerBase
{
    readonly ILogger<UsersController> _logger;
    readonly IUsersService _users_service;

    /// <summary>
    /// Пользователи
    /// </summary>
    public UsersController(ILogger<UsersController> logger, IUsersService users_service)
    {
        _logger = logger;
        _users_service = users_service;
    }

    /// <summary>
    /// Пользователи
    /// </summary>
    [HttpGet($"{GlobalStatic.Routes.GET}/{GlobalStatic.Routes.LIST}")]
    public async Task<UsersPaginationResponseModel> UsersGetList([FromQuery] UserListGetModel req, CancellationToken cancellation_token = default) => await _users_service.UsersGetList(req, cancellation_token);

    /// <summary>
    /// Пользователь
    /// </summary>
    [HttpGet($"{GlobalStatic.Routes.GET}/{{telegram_id}}")]
    public async Task<UserResponseModel> UserGet([FromRoute] long telegram_id, CancellationToken cancellation_token = default) => await _users_service.GetUser(telegram_id, cancellation_token);

    /// <summary>
    /// Обновить параметры пользователя
    /// </summary>
    [HttpPut($"{GlobalStatic.Routes.UPDATE}")]
    public async Task<ResponseBaseModel> UserUpdate(UpdateUserModel req, CancellationToken cancellation_token = default) => await _users_service.UpdateUser(req, cancellation_token);
}