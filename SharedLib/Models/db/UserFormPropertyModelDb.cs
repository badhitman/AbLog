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
    public string Code { get; set; } = default!;

    /// <summary>
    /// 
    /// </summary>
    public UserFormModelDb? OwnerForm { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int OwnerFormId { get; set; }

    /// <summary>
    /// Значение поля формы
    /// </summary>
    public string? PropValue { get; set; }
}