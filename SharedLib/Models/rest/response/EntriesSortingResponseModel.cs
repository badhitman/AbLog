////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Базовые элементы [Entry] с полем сортиировки. Ответ rest/api
/// </summary>
public class EntriesSortingResponseModel : ResponseBaseModel
{
    /// <summary>
    /// Базовые элементы [Entry] с полем сортиировки.
    /// </summary>
    public EntrySortingModel[]? Entries { get; set; }
}