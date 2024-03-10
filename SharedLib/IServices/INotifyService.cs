namespace SharedLib;

/// <summary>
/// Уведомления
/// </summary>
public interface INotifyService
{
    /// <summary>
    /// User notify handler
    /// </summary>
    public delegate void UserNotifyHandler(UserResponseModel user);

    /// <summary>
    /// Событие уведомления
    /// </summary>
    public event UserNotifyHandler? Notify;


    /// <summary>
    /// Проверка пользователя Telegram
    /// </summary>
    public void CheckTelegramUser(UserResponseModel user);
}
