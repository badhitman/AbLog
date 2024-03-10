////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// Конфигурация TelegramBot
/// </summary>
public class TelegramBotConfigModel
{
    /// <summary>
    /// Token TelegramBot
    /// </summary>
    [Required]
    public string? TelegramBotToken { get; set; }

    /// <summary>
    /// Автостарт
    /// </summary>
    public bool AutoStart { get; set; }

    /// <inheritdoc/>
    public static bool operator ==(TelegramBotConfigModel l, TelegramBotConfigModel r)
    {
        return
            l.AutoStart == r.AutoStart &&
            ((l.TelegramBotToken?.Equals(r.TelegramBotToken) == true) || (l.TelegramBotToken is null && r.TelegramBotToken is null));
    }

    /// <inheritdoc/>
    public static bool operator !=(TelegramBotConfigModel l, TelegramBotConfigModel r)
    {
        return !(l == r);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
            return true;

        if (obj is null)
            return false;

        return this == (TelegramBotConfigModel)obj;
    }

    /// <inheritdoc/>
    public override int GetHashCode() => $"{TelegramBotToken}{AutoStart}".GetHashCode();
}