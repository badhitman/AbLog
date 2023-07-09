////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Задача (выполнения скрипта)
/// </summary>
public class TaskModelDB : EntryModel
{
    /// <summary>
    /// Тип запуска/инициализации задачи: ручной, тригер, Telegram
    /// </summary>
    public TaskInitiatorsTypesEnum TaskInitiatorType { get; set; }

    /// <summary>
    /// FK: Инициализатора задачи/скрипта
    /// </summary>
    public int TaskInitiatorId { get; set; } = default!;

    /// <summary>
    /// Дата/время запуска/иниализации
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// Скрипт
    /// </summary>
    public ScriptModelDB? Script { get; set; }
    /// <summary>
    /// FK: Скрипт
    /// </summary>
    public int ScriptId { get; set; }

    /// <summary>
    /// Отчёт о выполнении задачи
    /// </summary>
    public List<ReportModelDB>? Reports { get; set; }
    /// <summary>
    /// FK: Отчёт о выполнении задачи
    /// </summary>
    public int? ReportId { get; set; }

    /// <summary>
    /// Дата/время 
    /// </summary>
    public DateTime FinishedAt { get; set; }
}