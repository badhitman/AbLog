////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Триггер запуска скрипта
/// </summary>
public class TrigerModelDB : EntryDescriptionModel
{
    /// <summary>
    /// Условия/ограничения запуска скрипта
    /// </summary>
    public List<TrigerConditionModelDB>? Conditions { get; set; }

    /// <summary>
    /// FK: Скрипт
    /// </summary>
    public int ScriptId { get; set; }

    /// <summary>
    /// Скрипт
    /// </summary>
    public ScriptModelDB? Script { get; set; }

    /// <summary>
    /// Включён/отключён триггер
    /// </summary>
    public bool IsDisable { get; set; } = false;
}