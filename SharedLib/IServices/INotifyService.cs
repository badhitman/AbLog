namespace SharedLib;

/// <summary>
/// Уведомления
/// </summary>
public interface INotifyService
{
    #region user`s (telegram)
    /// <summary>
    /// User notify handler
    /// </summary>
    public delegate void UserNotifyHandler(TResponseModel<UserModelDB> user);

    /// <summary>
    /// Событие уведомления
    /// </summary>
    public event UserNotifyHandler? Notify;

    /// <summary>
    /// Проверка пользователя Telegram
    /// </summary>
    public void CheckTelegramUser(TResponseModel<UserModelDB> user);
    #endregion

    #region mqtt debug
    /// <summary>
    /// Mqtt debug handler
    /// </summary>
    public delegate void MqttDebugHandler((TimeSpan Duration, string? Message, string TopicName) sender);

    /// <summary>
    /// Mqtt debug notify
    /// </summary>
    public event MqttDebugHandler? MqttDebugNotify;

    /// <summary>
    /// Mqtt debug
    /// </summary>
    public void MqttDebug((TimeSpan Duration, string? Message, string TopicName) sender);
    #endregion
}
