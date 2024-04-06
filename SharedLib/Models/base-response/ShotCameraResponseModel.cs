////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Снимок с камеры в формате byte[]
/// </summary>
public class ShotCameraResponseModel : ResponseBaseModel
{
    /// <summary>
    /// Снимок с камеры в формате byte[]
    /// </summary>
    public ShotCameraModel ShotCameraImage { get; set; }
}