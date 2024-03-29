﻿////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Снимок с камеры
/// </summary>
/// <param name="Data">Данные фото</param>
/// <param name="Format">Расширение</param>
public record struct ShotCameraModel(byte[] Data, string Format);