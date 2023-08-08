////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Telegram.Bot.Types;
using SharedLib;

namespace ServerLib;

/// <summary>
/// 
/// </summary>
public interface ITelegramBotFormFillingServive
{
    /// <summary>
    /// 
    /// </summary>
    public Task<Message> FormFillingHandle(UserFormModelDb form, int message_id, TypeValueTelegramBotHandle type_handler, string? set_value, CancellationToken cancellation_token = default);
}