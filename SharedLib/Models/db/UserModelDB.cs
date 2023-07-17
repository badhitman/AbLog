////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// Пользователь системы
/// </summary>
[Index(nameof(TelegramId), IsUnique = true)]
public class UserModelDB : UniversalModelDB
{
    /// <summary>
    /// Email пользователя
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Идентификатор Telegram (внутренний/технический. не чпу)
    /// </summary>
    public long TelegramId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool IsDisabled { get; set; } = true;

    /// <summary>
    /// 
    /// </summary>
    public bool AllowSystemCommands { get; set; } = true;

    /// <summary>
    /// 
    /// </summary>
    public string FirstName { get; set; } = default!;

    /// <summary>
    /// 
    /// </summary>
    public DateTime LastUpdate { get; set; } = DateTime.Now;

    /// <summary>
    /// 
    /// </summary>
    public string? LastName { get; set; }
}