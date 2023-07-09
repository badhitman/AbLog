////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.Net;

namespace SharedLib;

/// <summary>
/// Универсальная модель:
/// с признаками AlarmSubscriber (подписка на уведомления) и CommandsAllowed (поддержка команд)
/// </summary>
[Index(nameof(AlarmSubscriber), nameof(CommandsAllowed))]
public abstract class UniversalModelDB : EntryModel
{
    /// <summary>
    /// Подписка на уведомления
    /// </summary>
    public bool AlarmSubscriber { get; set; }

    /// <summary>
    /// Поддержка команд
    /// </summary>
    public bool CommandsAllowed { get; set; }
}