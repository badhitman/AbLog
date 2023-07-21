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
    public Task FormFillingHandle(UserFormModelDb form, string? set_value, CancellationToken cancellation_token = default);
}
