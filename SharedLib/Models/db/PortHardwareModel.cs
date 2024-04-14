////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Порт устройства
/// </summary>
public class PortHardwareModel : PortHardwareBaseModel
{
    /// <inheritdoc/>
    public static PortHardwareModel Build(PortModelDB db_port)
    {
        return new PortHardwareModel()
        {
            Id = db_port.Id,
            PortNum = db_port.PortNum,
            Name = db_port.Name,
            Hardware = new EntryModel() { Id = db_port.Hardware!.Id, Name = db_port.Hardware.Name },
        };
    }

    /// <summary>
    /// Управляющий блок
    /// </summary>
    public EntryModel? Hardware { get; set; }
}