////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Снимок с камеры в формате Base64
/// </summary>
public class ShotCameraAsBase64ResponseModel : ResponseBaseModel
{
    /// <summary>
    /// Снимок с камеры в формате Base64
    /// </summary>
    public ShotCameraAsBase64Model ShotCameraImage { get; set; }
}