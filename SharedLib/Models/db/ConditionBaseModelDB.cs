////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <inheritdoc/>
public abstract class ConditionBaseModelDB : ConditionAnonimModel
{
    /// <summary>
    /// FK: Владельца условия/ограничения (команда или тригер)
    /// </summary>
    public int OwnerId { get; set; }

    /// <summary>
    /// Проверка условия
    /// </summary>
    public ResponseBaseModel ValidateCondition
    {
        get
        {
            ResponseBaseModel res = new();
            if (OwnerId < 1)
            {
                res.AddError("Ошибка {EA3239C3-ABE8-4B84-97C7-4DA0F9A6CBA7}");
                return res;
            }

            if (HardwareId < 1)
            {
                res.AddError("Не выбрано устройство");
            }
            if (PortId < 1)
            {
                res.AddError("Не выбран порт");
            }
            if (string.IsNullOrWhiteSpace(Value))
            {
                res.AddError("Установите значение");
            }
            if (ConditionValueType == СomparisonsValuesTypesEnum.ValueAsDecimal)
            {
                if (string.IsNullOrWhiteSpace(Value))
                    Value = "0";

                if (Value.StartsWith('.') || Value.StartsWith(','))
                    Value = $"0{Value}";

                if (Value.EndsWith('.') || Value.EndsWith(','))
                    Value = $"{Value}0";

                if (!double.TryParse(Value, out _))
                    res.AddError("Значение числа не валидное");
            }
            return res;
        }
    }
}