////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using SharedLib;
using Telegram.Bot.Types;

namespace ServerLib;

/// <summary>
/// Заполнение формы данными через TelegramBot
/// </summary>
public interface ITelegramBotFormFillingServive
{
    /// <summary>
    /// Ввод данных
    /// </summary>
    public Task<Message> FormFillingHandle(UserFormModelDb form, int message_id, TypeValueTelegramBotHandle type_handler, string? set_value, CancellationToken cancellation_token = default);
}