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
    /// <inheritdoc/>
    public string? Email { get; set; }

    /// <summary>
    /// Идентификатор Telegram (внутренний/технический. не чпу)
    /// </summary>
    public long TelegramId { get; set; }

    /// <inheritdoc/>
    public bool IsDisabled { get; set; } = true;

    /// <summary>
    /// Разрешён доступ к системному меню команд
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
    /// MessageId (telegram)
    /// </summary>
    public int MessageId { get; set; }

    /// <inheritdoc/>
    public string? FirstName { get; set; }

    /// <inheritdoc/>
    public DateTime LastUpdate { get; set; } = DateTime.Now;

    /// <inheritdoc/>
    public string? LastName { get; set; }

    /// <summary>
    /// Форма, которую пользователь заполняет чрез TelegramBot
    /// </summary>
    public UserFormModelDb? UserForm { get; set; }

    /// <inheritdoc/>
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