////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Обновление условия/ограничения значения порта (для команды или тригера)
/// </summary>
public class UpdateConditionRequestModel : ConditionBaseModelDB
{
    /// <summary>
    /// Обновление условия/ограничения значения порта (для команды или тригера)
    /// </summary>
    public UpdateConditionRequestModel() { }

    /// <summary>
    /// Обновление условия/ограничения значения порта (для команды или тригера)
    /// </summary>
    public UpdateConditionRequestModel(ConditionAnonimModel init)
    {
        this.Id = init.Id;
        this.Name = init.Name;
        this.HardwareId = init.HardwareId;
        this.PortId = init.PortId;
        this.ConditionValueType = init.ConditionValueType;
        this.СomparisonMode = init.СomparisonMode;
        this.Value = init.Value;
    }

    /// <summary>
    /// Типы условных требований: команда, тригер ...
    /// </summary>
    public ConditionsTypesEnum ConditionType { get; set; }
}
