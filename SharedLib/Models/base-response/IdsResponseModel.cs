////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Перечень идентификаторов: Ответ rest/api
/// </summary>
public class IdsResponseModel : ResponseBaseModel
{
    /// <summary>
    /// Перечень идентификаторов
    /// </summary>
    public IEnumerable<int>? Ids { get; set; }
}