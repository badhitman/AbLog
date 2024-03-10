////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Update user
/// </summary>
public class UpdateUserModel
{
    /// <summary>
    /// TelegramId
    /// </summary>
    public long TelegramId { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Allow system commands
    /// </summary>
    public bool AllowSystemCommands { get; set; } = true;

    /// <summary>
    /// Is disabled
    /// </summary>
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Allow alerts
    /// </summary>
    public bool AllowAlerts { get; set; }

    /// <summary>
    /// Allow hardware control
    /// </summary>
    public bool AllowHardwareControl { get; set; }

    /// <summary>
    /// Allow change MQTT config
    /// </summary>
    public bool AllowChangeMqttConfig { get; set; }
}