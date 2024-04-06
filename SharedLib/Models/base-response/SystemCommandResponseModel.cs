////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Системная команда (patch parameters)
/// </summary>
public class SystemCommandResponseModel : ResponseBaseModel
{
    /// <summary>
    /// Системная команда (patch parameters)
    /// </summary>
    public SystemCommandModelDB? SystemCommand { get; set; }
}