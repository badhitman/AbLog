////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Пользователи
/// </summary>
public interface IUsersService
{
    /// <summary>
    /// Получить пользователей
    /// </summary>
    public Task<UsersPaginationResponseModel> UsersGetList(UserListGetModel req, CancellationToken cancellation_token = default);

    /// <summary>
    /// Получить пользователя
    /// </summary>
    public Task<UserResponseModel> GetUser(long telegram_id, CancellationToken cancellation_token = default);

    /// <summary>
    /// Установть настройки пользователю
    /// </summary>
    public Task<ResponseBaseModel> UpdateUser(UpdateUserModel req, CancellationToken cancellation_token = default);
}