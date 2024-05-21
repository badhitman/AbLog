////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Снимок с камеры
/// </summary>
public class ShotCameraAsBase64Model(string data64, string format)
{
    /// <inheritdoc/>
    public string Data64 { get; set; } = data64;

    /// <inheritdoc/>
    public string Format { get; set; } = format;
}