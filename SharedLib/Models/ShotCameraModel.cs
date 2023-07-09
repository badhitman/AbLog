////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Снимок с камеры
/// </summary>
/// <param name="data64">Данные Base64</param>
/// <param name="format">Расширение</param>
public record struct ShotCameraAsBase64Model(string data64, string format);

/// <summary>
/// Снимок с камеры
/// </summary>
/// <param name="data">Данные фото</param>
/// <param name="format">Расширение</param>
public record struct ShotCameraModel(byte[] data, string format);