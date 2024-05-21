////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Снимок с камеры
/// </summary>
public class ShotCameraModel(byte[] data, string format)
{
    /// <inheritdoc/>
    public byte[] Data { get; set; } = data;

    /// <inheritdoc/>
    public string Format { get; set; } = format;
}