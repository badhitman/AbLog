////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Команда скрипта
/// </summary>
public class CommandResponseModel : ResponseBaseModel
{
    /// <summary>
    /// Команда скрипта
    /// </summary>
    public CommandModelDB? Command { get; set; }
}