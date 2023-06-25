﻿using SharedLib;
using Refit;

namespace BlazorLib;

/// <summary>
/// Работа с бэком: Cameras
/// </summary>
[Headers("Content-Type: application/json")]
public interface IRefitCamerasService
{
    /// <summary>
    /// Получить все доступные камеры
    /// </summary>
    [Get($"/api/{GlobalStatic.HttpRoutes.Cameras}/{GlobalStatic.HttpRoutes.LIST}")]
    public Task<ApiResponse<EntriesSortingResponseModel>> GetAllCameras();
}