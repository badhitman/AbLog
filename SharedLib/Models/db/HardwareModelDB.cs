////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Управляющий блок (устройство)
/// </summary>
public class HardwareModelDB : HardwareBaseModel
{
    /// <inheritdoc/>
    public static HardwareModelDB Build(HardwareBaseModel hardware)
    {
        return new HardwareModelDB()
        {
            Name = hardware.Name,
            Address = hardware.Address,
            Password = hardware.Password,
            AlarmSubscriber = hardware.AlarmSubscriber,
            CommandsAllowed = hardware.CommandsAllowed,
            Ports = [],
        };
    }

    /// <summary>
    /// Порты устройства
    /// </summary>
    public List<PortModelDB>? Ports { get; set; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{Name} ({Address})";
    }
}