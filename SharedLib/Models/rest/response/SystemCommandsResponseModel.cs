////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// 
/// </summary>
public class SystemCommandsResponseModel : ResponseBaseModel
{
    /// <summary>
    /// 
    /// </summary>
    public IEnumerable<SystemCommandModelDB>? SystemCommands { get; set; }
}