////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Управляющий блок (устройство)
/// </summary>
public class HardwareModelDB : HardwareBaseModel
{
    /// <summary>
    /// Управляющий блок (устройство)
    /// </summary>
    public HardwareModelDB() { }

    /// <summary>
    /// Управляющий блок (устройство)
    /// </summary>
    public HardwareModelDB(HardwareBaseModel hardware)
    {
        Name = hardware.Name;
        Address = hardware.Address;
        Password = hardware.Password;
        AlarmSubscriber = hardware.AlarmSubscriber;
        CommandsAllowed = hardware.CommandsAllowed;
        Ports = [];
    }

    /// <summary>
    /// Порты устройства
    /// </summary>
    public List<PortModelDB>? Ports { get; set; }

    /// <inheritdoc/>
    public override string ToString()
    {
        string hw_name = $"{Name} ({Address})";
        return hw_name;
    }
}