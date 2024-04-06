////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Порт устройства
/// </summary>
public class PortHardwareResponseModel : ResponseBaseModel
{
    /// <summary>
    /// Порт устройства
    /// </summary>
    public PortHardwareModel? Port { get; set; }
}