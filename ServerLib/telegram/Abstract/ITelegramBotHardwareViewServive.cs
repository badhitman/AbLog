////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Telegram.Bot.Types;

namespace ServerLib;

/// <summary>
/// Просмотр контроллеров через TelegramBot
/// </summary>
public interface ITelegramBotHardwareViewServive
{
    /// <summary>
    /// Прочитать состояние контроллера
    /// </summary>
    public Task<Message> HardwareViewMainHandle(long chat_id, int message_id, string? set_value, int hardware_id, CancellationToken cancellation_token = default);

    /// <summary>
    /// Прочитать состояние порта контроллрера
    /// </summary>
    /// <param name="chat_id">чат</param>
    /// <param name="message_id">сообщение</param>
    /// <param name="set_value">(опционально) установка значения - отправка команды контроллеру</param>
    /// <param name="port_id">порт</param>
    /// <param name="cancellation_token"></param>
    public Task<Message> HardwarePortViewHandle(long chat_id, int message_id, string? set_value, int port_id, CancellationToken cancellation_token = default);
}