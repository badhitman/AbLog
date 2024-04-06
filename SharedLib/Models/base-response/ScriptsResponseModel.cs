////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Перечень скриптов
/// </summary>
public class ScriptsResponseModel : ResponseBaseModel
{
    /// <summary>
    /// Скрипты
    /// </summary>
    public List<ScriptModelDB>? Scripts { get; set; }
}