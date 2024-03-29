﻿namespace SharedLib;

/// <summary>
/// Уведомления
/// </summary>
public interface INotifyService
{
    #region user`s (telegram)
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
    #endregion

    #region mqtt debug
    /// <summary>
    /// 
    /// </summary>
    public delegate void MqttDebugHandler((TimeSpan Duration, string? Message, string TopicName) sender);

    /// <summary>
    /// 
    /// </summary>
    public event MqttDebugHandler? MqttDebugNotify;

    /// <summary>
    /// 
    /// </summary>
    public void MqttDebug((TimeSpan Duration, string? Message, string TopicName) sender);
    #endregion
}
