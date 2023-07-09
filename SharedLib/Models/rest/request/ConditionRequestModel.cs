////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Запрос условия/ограничения значения порта (для тригера или команда)
/// </summary>
public class ConditionRequestModel
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