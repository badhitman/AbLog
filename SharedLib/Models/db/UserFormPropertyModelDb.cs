////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Свойство формы (заполняемая через TelegramBot)
/// </summary>
public class UserFormPropertyModelDb : EntryModel
{
    /// <summary>
    /// имя поля формы
    /// </summary>
    public required string Code { get; set; }

    /// <summary>
    /// Форма для заполнения через TelegramBot.
    /// </summary>
    public UserFormModelDb? OwnerForm { get; set; }
    /// <summary>
    /// FK: Форма для заполнения через TelegramBot.
    /// </summary>
    public int OwnerFormId { get; set; }

    /// <summary>
    /// Значение поля формы
    /// </summary>
    public string? PropValue { get; set; }
}