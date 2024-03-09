////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Перечень скриптов: Ответ rest/api
/// </summary>
public class ScriptsResponseModel : ResponseBaseModel
{
    /// <summary>
    /// Скрипты
    /// </summary>
    public List<ScriptModelDB>? Scripts { get; set; }
}