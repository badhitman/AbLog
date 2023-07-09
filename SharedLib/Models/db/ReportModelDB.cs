////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Отчёт выполнения задачи скрипта
/// </summary>
public class ReportModelDB : EntryModel
{
    /// <summary>
    /// Дата/время формирования отчёта
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// Задача, для которого создан отчёт
    /// </summary>
    public TaskModelDB? OwnerTask { get; set; }
    /// <summary>
    /// FK: Задача, для которого создан отчёт
    /// </summary>
    public int OwnerTaskId { get; set; }

    /// <summary>
    /// Результат выполнения
    /// </summary>
    public bool Success { get; set; }
}