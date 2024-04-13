////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// Системная команда
/// </summary>
public class SystemCommandModelDB : EntryModel
{
    /// <summary>
    /// Файл запуска
    /// </summary>
    [Required]
    public string? FileName { get; set; }

    /// <summary>
    /// Параметры запуска
    /// </summary>
    public string? Arguments { get; set; }

    /// <summary>
    /// Команда отключена?
    /// </summary>
    public bool IsDisabled { get; set; }
}