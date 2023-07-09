////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Тригеры (все): Ответ rest/api
/// </summary>
public class TriggersResponseModel : ResponseBaseModel
{
    /// <summary>
    /// Перечень тригеров
    /// </summary>
    public IEnumerable<TrigerModelDB>? Triggers { get; set; }
}