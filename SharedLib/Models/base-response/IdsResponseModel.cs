////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Перечень идентификаторов
/// </summary>
public class IdsResponseModel : ResponseBaseModel
{
    /// <summary>
    /// Перечень идентификаторов
    /// </summary>
    public IEnumerable<int>? Ids { get; set; }
}