////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Mvc;
using NLog;
using SharedLib;

namespace ABLogWeb;

/// <summary>
/// 
/// </summary>
[Route("/api/[controller]")]
[ApiController]
public class CamerasController : ControllerBase
{
    readonly Logger _logger = LogManager.GetCurrentClassLogger();
    readonly ICamerasService _cam;

    /// <summary>
    /// 
    /// </summary>
    public CamerasController(Logger logger, ICamerasService cam)
    {
        _logger = logger;
        _cam = cam;
    }

    /// <summary>
    /// 
    /// </summary>
    [HttpGet("photo/{index_cam}/{characteristic?}")]
    public async Task<IActionResult> Poto(int? index_cam, string? characteristic)
    {
        ShotCameraResponseModel photo = await _cam.TakeOneShotAsync(index_cam, characteristic);
        byte[] data_bytes;
        string content_type;
        if (!photo.IsSuccess)
        {
            data_bytes = System.IO.File.ReadAllBytes("wwwroot/img/no-webcam-icon.jpg");
            content_type = $"image/jpg";
        }
        else
        {
            data_bytes = photo.ShotCameraImage.data;
            content_type = $"image/{photo.ShotCameraImage.format[1..]}";
        }

        return File(data_bytes, content_type, $"photo{photo.ShotCameraImage.format[1..]}");
    }
}