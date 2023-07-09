////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Тригер запуска скрипта
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
    /// Включён/отключён тригер
    /// </summary>
    public bool IsDisable { get; set; } = false;
}