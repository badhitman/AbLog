////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Управляющий блок (устройство) - View модель
/// </summary>
public class HardwareModel : HardwareBaseModel
{
    /// <inheritdoc/>
    public static new HardwareModel Build(HardwareModelDB v)
    {
        return new HardwareModel()
        {
            Id = v.Id,
            Name = v.Name,
            Address = v.Address,
            AlarmSubscriber = v.AlarmSubscriber,
            CommandsAllowed = v.CommandsAllowed,
            Password = v.Password,
            Ports = v.Ports?.Select(p => PortHardwareModel.Build(p)).OrderBy(x => x.PortNum).ToList(),
        };
    }

    /// <summary>
    /// Порты устройства
    /// </summary>
    public List<PortHardwareModel>? Ports { get; set; }
}