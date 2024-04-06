////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Базовые элементы [Entry] вложенной структуры
/// </summary>
public class EntriesNestedResponseModel : ResponseBaseModel
{
    /// <summary>
    /// Базовые элементы [Entry] вложенной структуры.
    /// </summary>
    public EntryNestedModel[]? Entries { get; set; }
}