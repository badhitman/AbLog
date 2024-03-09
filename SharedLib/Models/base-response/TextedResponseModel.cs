////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Базовый ответ строкой
/// </summary>
public class TextedResponseModel : ResponseBaseModel
{
    /// <summary>
    /// Text
    /// </summary>
    public string? TextPayload { get; set; }
}