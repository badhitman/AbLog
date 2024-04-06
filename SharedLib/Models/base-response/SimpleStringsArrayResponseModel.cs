////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Универсальная модель ответа коллекцией строк
/// </summary>
public class SimpleStringsArrayResponseModel : ResponseBaseModel
{
    /// <summary>
    /// Данные ответа
    /// </summary>
    public IEnumerable<string>? Elements { get; set; }
}