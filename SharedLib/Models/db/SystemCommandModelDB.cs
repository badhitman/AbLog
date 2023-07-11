////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// Системная команда
/// </summary>
public class SystemCommandModelDB
{
    /// <summary>
    /// Ключ/идентификатор
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Файл запуска
    /// </summary>
    public string FileName { get; set; } = default!;

    /// <summary>
    /// Параметры запуска
    /// </summary>
    public string? Arguments { get; set; }

    /// <summary>
    /// Команда отключена?
    /// </summary>
    public bool IsDisabled { get; set; }
}