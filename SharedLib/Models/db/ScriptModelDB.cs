////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Скрипт (пакет команд/заданий)
/// </summary>
public class ScriptModelDB : EntryDescriptionModel
{
    /// <summary>
    /// Команды скрипта
    /// </summary>
    public List<CommandModelDB>? Commands { get; set; }

    /// <summary>
    /// Конкуренты скрипта
    /// </summary>
    public IEnumerable<ContentionsModelDB>? Contentions { get; set; }

    /// <summary>
    /// Триггеры скрипта
    /// </summary>
    public IEnumerable<TrigerModelDB>? Triggers { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <inheritdoc/>
    public override string ToString() => $"[script:#{Id} '{Name}]'{(Commands is not null && Commands.Count != 0 ? $"({Commands.Count} команд)" : "")}";
}