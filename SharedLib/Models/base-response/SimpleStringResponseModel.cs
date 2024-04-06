////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Simple string - response
/// </summary>
public class SimpleStringResponseModel : ResponseBaseModel
{
    /// <summary>
    /// Данные ответа
    /// </summary>
    public string? TextPayload { get; set; }
}