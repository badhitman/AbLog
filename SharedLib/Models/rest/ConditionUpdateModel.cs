////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Обновление условия/ограничения значения порта (для команды или тригера)
/// </summary>
public class ConditionUpdateModel : ConditionBaseModelDB
{
    /// <inheritdoc/>
    public static ConditionUpdateModel Build(ConditionAnonimModel init, ConditionsTypesEnum ConditionType, int OwnerId)
    {
        return new ConditionUpdateModel()
        {
            Id = init.Id,
            Name = init.Name,
            HardwareId = init.HardwareId,
            PortId = init.PortId,
            ConditionValueType = init.ConditionValueType,
            СomparisonMode = init.СomparisonMode,
            Value = init.Value,
            ConditionType = ConditionType,
            OwnerId = OwnerId
        };
    }

    /// <summary>
    /// Типы условных требований: команда, тригер ...
    /// </summary>
    public ConditionsTypesEnum ConditionType { get; set; }
}