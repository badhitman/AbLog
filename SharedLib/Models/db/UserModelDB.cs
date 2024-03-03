////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// Пользователь системы
/// </summary>
[Index(nameof(TelegramId), IsUnique = true)]
public class UserModelDB : UniversalModelDB
{
    /// <summary>
    /// Email пользователя
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Идентификатор Telegram (внутренний/технический. не чпу)
    /// </summary>
    public long TelegramId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool IsDisabled { get; set; } = true;

    /// <summary>
    /// Разрешён доступ к системному меню комманд
    /// </summary>
    public bool AllowSystemCommands { get; set; } = true;

    /// <summary>
    /// Разрешается менять настройки MQTT
    /// </summary>
    public bool AllowChangeMqttConfig { get; set; } = true;

    /// <summary>
    /// Telegram chat id
    /// </summary>
    public long ChatId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int MessageId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string FirstName { get; set; } = default!;

    /// <summary>
    /// 
    /// </summary>
    public DateTime LastUpdate { get; set; } = DateTime.Now;

    /// <summary>
    /// 
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public UserFormModelDb? UserForm { get; set; }

    /// <summary>
    /// Reload
    /// </summary>
    public void Reload(UserModelDB elementBeforeEdit)
    {
        Email = elementBeforeEdit.Email;
        AlarmSubscriber = elementBeforeEdit.AlarmSubscriber;
        CommandsAllowed = elementBeforeEdit.CommandsAllowed;
        IsDisabled = elementBeforeEdit.IsDisabled;
        AllowSystemCommands = elementBeforeEdit.AllowSystemCommands;
        TelegramId = elementBeforeEdit.TelegramId;
        LastName = elementBeforeEdit.LastName;
        FirstName = elementBeforeEdit.FirstName;
        Name = elementBeforeEdit.Name;
        LastUpdate = elementBeforeEdit.LastUpdate;
        UserForm = elementBeforeEdit.UserForm;
    }
}