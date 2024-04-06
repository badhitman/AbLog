////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Перечень устройств
/// </summary>
public class HardwaresResponseModel : ResponseBaseModel
{
    /// <summary>
    /// Перечень устройств
    /// </summary>
    public IEnumerable<HardwareModel>? Hardwares { get; set; }
}