////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Условие. Ограничение, зависящее от заданного значения/состояния заданного порта
/// </summary>
public class ConditionAnonimModel : EntryModel
{
    /// <summary>
    /// FK:Управляющий блок
    /// </summary>
    public int HardwareId { get; set; }
    /// <summary>
    /// Управляющий блок
    /// </summary>
    public HardwareModelDB? Hardware { get; set; }

    /// <summary>
    /// FK:Порт блока
    /// </summary>
    public int PortId { get; set; }
    /// <summary>
    /// Порт блока
    /// </summary>
    public PortModelDB? Port { get; set; }

    /// <summary>
    /// Тип значения для сравнивания (как строка, как число)
    /// </summary>
    public СomparisonsValuesTypesEnum ConditionValueType { get; set; }

    /// <summary>
    /// Режимы сравнения (равно, не равно...)
    /// </summary>
    public СomparisonsModesEnum СomparisonMode { get; set; }

    /// <summary>
    /// Значение порта (для проверки условия)
    /// </summary>
    public string Value { get; set; } = string.Empty;
}