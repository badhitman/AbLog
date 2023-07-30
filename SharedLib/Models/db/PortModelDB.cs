////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Порт устройства
/// </summary>
public class PortModelDB : PortHardwareBaseModel
{
    /// <summary>
    /// FK: Устройство
    /// </summary>
    public int HardwareId { get; set; }

    /// <summary>
    /// Устройство
    /// </summary>
    public HardwareModelDB? Hardware { get; set; }

    /// <summary>
    /// Порт деактивирован
    /// </summary>
    public bool IsDisable { get; set; }

    /// <inheritdoc/>
    public override string ToString()
    {
        string port_name;
        if (string.IsNullOrWhiteSpace(Name))
        {
            port_name = $"P{PortNum}";
        }
        else
        {
            port_name = $"{Name} (P{PortNum})";
        }
        return port_name;
    }
}