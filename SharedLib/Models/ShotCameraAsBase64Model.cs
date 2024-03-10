////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Снимок с камеры
/// </summary>
/// <param name="Data64">Данные Base64</param>
/// <param name="Format">Расширение</param>
public record struct ShotCameraAsBase64Model(string Data64, string Format);