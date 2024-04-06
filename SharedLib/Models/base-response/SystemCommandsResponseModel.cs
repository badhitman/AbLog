////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Системнаые команды (patch parameters)
/// </summary>
public class SystemCommandsResponseModel : ResponseBaseModel
{
    /// <summary>
    /// Системнаые команды (patch parameters)
    /// </summary>
    public IEnumerable<SystemCommandModelDB>? SystemCommands { get; set; }
}