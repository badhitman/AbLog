////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Условие/ограничения (для тригера или команды)
/// </summary>
public class ConditionObjectModel
{
    /// <summary>
    /// Идентификатор объекта ограничения/условия
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Тип ограничения: команда или скрипт
    /// </summary>
    public ConditionsTypesEnum ConditionType { get; set; }
}