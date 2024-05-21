////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using System.ComponentModel;

namespace SharedLib;

/// <summary>
/// Типы условных требований: команда, триггер ...
/// </summary>
public enum ConditionsTypesEnum
{
    /// <summary>
    /// Для команды
    /// </summary>
    [Description("Команда")]
    Command = 10,

    /// <summary>
    /// Для триггера
    /// </summary>
    [Description("Триггер")]
    Trigger = 20
}
