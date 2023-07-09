////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Telegram пользователь
/// </summary>
public class TelegramUserModelDB : EntryModel
{
    /// <summary>
    /// Идентификатор пользователя Telegram
    /// </summary>
    public long TelegramId { get; set; }

    /// <summary>
    /// Юзернэйм
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// Связанный пользователь системы
    /// </summary>
    public UserModelDB? LinkedUser { get; set; }
    /// <summary>
    /// FK: Связанный пользователь системы
    /// </summary>
    public int? LinkedUserId { get; set; }

    /// <inheritdoc/>
    public override string ToString()
    {
        string full_name = string.IsNullOrWhiteSpace(Name) ? "" : $"{Name}; ";
        string user_name = string.IsNullOrWhiteSpace(UserName) ? "" : $"@{UserName}; ";
        string ret_name = $"{full_name}{user_name}";// tid: {TelegramId};
        return ret_name;
    }
}