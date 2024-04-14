////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Newtonsoft.Json;

namespace SharedLib;

/// <summary>
/// Результат запроса создания объекта (ключ создаваемого объекта: string)
/// </summary>
public class CreateObjectOfStringKeyResponseModel : ResponseBaseModel
{
    /// <summary>
    /// Идентификатор нового созданного объекта
    /// </summary>
    [JsonProperty("id")]
    public string? Id { get; set; }
}