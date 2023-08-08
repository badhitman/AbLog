////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Telegram.Bot.Types;

namespace ServerLib;

/// <summary>
/// 
/// </summary>
public interface ITelegramBotHardwareViewServive
{
    /// <summary>
    /// 
    /// </summary>
    public Task<Message> HardwareViewMainHandle(long chat_id, int message_id, string? set_value, int hardware_id, CancellationToken cancellation_token = default);

    /// <summary>
    /// 
    /// </summary>
    public Task<Message> HardwarePortViewHandle(long chat_id, int message_id, string? set_value, int port_id, CancellationToken cancellation_token = default);
}