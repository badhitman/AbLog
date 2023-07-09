////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Хранимый (в БД) параметр/значение
/// </summary>
public class ParametersStorageModelDB : EntryModel
{
    /// <summary>
    /// Значение параметра
    /// </summary>
    public string StoredValue { get; set; } = string.Empty;

    /// <summary>
    /// Имя типа данных параметра
    /// </summary>
    public string TypeName { get; set; } = string.Empty;

    /// <summary>
    /// Получить значение параметра как булево. Если тип параметра отличается от bool - вернёт NULL
    /// </summary>
    public bool? GetValueAsBool => TypeName.Equals(typeof(bool).Name, StringComparison.OrdinalIgnoreCase) ? bool.Parse(StoredValue) : null;

    /// <summary>
    /// Получить значение параметра как целое число. Если тип параметра отличается от int - вернёт NULL
    /// </summary>
    public int? GetValueAsInt => TypeName.Equals(typeof(int).Name, StringComparison.OrdinalIgnoreCase) ? int.Parse(StoredValue) : null;

    /// <summary>
    /// Получить значение параметра как дробное число. Если тип параметра отличается от long - вернёт NULL
    /// </summary>
    public long? GetValueAsLong => TypeName.Equals(typeof(long).Name, StringComparison.OrdinalIgnoreCase) ? long.Parse(StoredValue) : null;
}