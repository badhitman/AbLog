namespace SharedLib;

/// <summary>
/// 
/// </summary>
public class UpdateUserModel
{
    /// <summary>
    /// 
    /// </summary>
    public long TelegramId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool AllowSystemCommands { get; set; } = true;

    /// <summary>
    /// 
    /// </summary>
    public bool IsDisabled { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool AllowAlerts { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool AllowHardwareControl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool AllowChangeMqttConfig { get; set; }
}