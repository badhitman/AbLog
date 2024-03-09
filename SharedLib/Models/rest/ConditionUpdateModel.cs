////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Обновление условия/ограничения значения порта (для команды или тригера)
/// </summary>
public class ConditionUpdateModel : ConditionBaseModelDB
{
    /// <summary>
    /// Обновление условия/ограничения значения порта (для команды или тригера)
    /// </summary>
    public ConditionUpdateModel() { }

    /// <summary>
    /// Обновление условия/ограничения значения порта (для команды или тригера)
    /// </summary>
    public ConditionUpdateModel(ConditionAnonimModel init)
    {
        Id = init.Id;
        Name = init.Name;
        HardwareId = init.HardwareId;
        PortId = init.PortId;
        ConditionValueType = init.ConditionValueType;
        СomparisonMode = init.СomparisonMode;
        Value = init.Value;
    }

    /// <summary>
    /// Типы условных требований: команда, тригер ...
    /// </summary>
    public ConditionsTypesEnum ConditionType { get; set; }
}
