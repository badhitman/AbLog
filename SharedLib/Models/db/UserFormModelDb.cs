////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// Форма для заполнения через TelegramBot.
/// </summary>
public class UserFormModelDb
{
    /// <summary>
    /// Ключ/идентификатор
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Имя типа формы
    /// </summary>
    public string FormMapCode { get; set; } = default!;

    /// <summary>
    /// Пользователь, который заполняет форму
    /// </summary>
    public UserModelDB? OwnerUser { get; set; }

    /// <summary>
    /// [FK] Пользователь, который заполняет форму
    /// </summary>
    public int OwnerUserId { get; set; }

    /// <summary>
    /// Свойства формы
    /// </summary>
    public List<UserFormPropertyModelDb>? Properties { get; set; }
}