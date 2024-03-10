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
    /// 
    /// </summary>
    public event UserNotifyHandler? Notify;

    /// <summary>
    /// Notify: Check Telegram user
    /// </summary>
    public void CheckTelegramUser(UserResponseModel user)
    {
        Logger.LogDebug($"call > {nameof(Notify)}: {JsonConvert.SerializeObject(user, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore })}");
        Notify?.Invoke(user);
    }
}