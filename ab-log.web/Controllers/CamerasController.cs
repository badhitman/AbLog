﻿////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Mvc;
using SharedLib;

namespace ABLogWeb;

/// <summary>
/// Камеры USB
/// </summary>
[Route("/api/[controller]"), ApiController]
public class CamerasController(ICamerasService cam) : ControllerBase
{
    /// <summary>
    /// 
    /// </summary>
    [HttpGet($"/{GlobalStatic.Routes.Cameras}/{{index_cam}}/{{characteristic?}}")]
    public async Task<IActionResult> Poto([FromRoute] int? index_cam, [FromRoute] string? characteristic)
    {
        ShotCameraResponseModel photo = await cam.TakeOneShotAsync(index_cam, characteristic);
        byte[] data_bytes;
        string content_type;
        if (!photo.IsSuccess)
        {
            data_bytes = System.IO.File.ReadAllBytes("wwwroot/img/no-webcam-icon.jpg");
            content_type = $"image/jpg";
        }
        else
        {
            data_bytes = photo.ShotCameraImage.Data;
            content_type = $"image/{photo.ShotCameraImage.Format[1..]}";
        }

        return File(data_bytes, content_type, $"photo{photo.ShotCameraImage.Format[1..]}");
    }
}