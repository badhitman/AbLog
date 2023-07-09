////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Пользователь системы
/// </summary>
public class UserModelDB : UniversalModelDB
{
    /// <summary>
    /// Email пользователя
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Телефон (сотовый)
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Идентификатор Telegram (внутренний/технический. не чпу)
    /// </summary>
    public string? TelegramId { get; set; }

    /// <inheritdoc/>
    public override string ToString()
    {
        string ret_name = Name;
        if (!string.IsNullOrWhiteSpace(Email))
        {
            ret_name = $"{ret_name} [e:{Email}]";
        }
        if (!string.IsNullOrWhiteSpace(Phone))
        {
            ret_name = $"{ret_name} [t:{Phone}]";
        }

        return ret_name;
    }
}