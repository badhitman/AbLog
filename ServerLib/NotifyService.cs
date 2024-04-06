using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SharedLib;
using static SharedLib.INotifyService;

namespace ServerLib;

/// <summary>
/// Notify service
/// </summary>
public class NotifyService(ILogger<NotifyService> Logger) : INotifyService
{
    /// <summary>
    /// User notify handler
    /// </summary>
    public event UserNotifyHandler? Notify;

    /// <summary>
    /// MQTT отладка
    /// </summary>
    public event MqttDebugHandler? MqttDebugNotify;

    /// <summary>
    /// Notify: Check Telegram user
    /// </summary>
    public void CheckTelegramUser(UserResponseModel user)
    {
        Logger.LogDebug($"call > {nameof(Notify)}: {JsonConvert.SerializeObject(user, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore })}");
        Notify?.Invoke(user);
    }

    /// <summary>
    /// Notify: MQTT debug
    /// </summary>
    public void MqttDebug((TimeSpan Duration, string? Message, string TopicName) sender)
    {
        Logger.LogDebug($"call > {nameof(MqttDebug)}: {JsonConvert.SerializeObject(new { DurationMilliseconds = sender.Duration.TotalMilliseconds, sender.TopicName, sender.Message }, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore })}");
        MqttDebugNotify?.Invoke(sender);
    }
}