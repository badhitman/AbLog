﻿////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using FlashCap;
using Microsoft.Extensions.Logging;
using SharedLib;

namespace ServerLib;

/// <summary>
/// Flash Cam
/// </summary>
public class FlashCamLocalService(ILogger<FlashCamLocalService> logger) : ICamerasService
{

    /// <inheritdoc/>
    public IEnumerable<CameraModel> GetAvailableDevicesNames(string? find_name_camera = null)
    {
        int i = 0;
        return new CaptureDevices()
            .EnumerateDescriptors()
            .Where(x => string.IsNullOrWhiteSpace(find_name_camera) || x.Name.Contains(find_name_camera, StringComparison.OrdinalIgnoreCase))
            .Select(x => new CameraModel() { Id = i++, Name = x.Name, Characteristics = x.Characteristics.Select(x => x.ToString()) })
            .ToArray();
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<ShotCameraAsBase64Model>> TakeOneShotAsBase64Async(int? index_camera, string? characteristic)
    {
        TResponseModel<ShotCameraAsBase64Model> res = new();

        TResponseModel<ShotCameraModel> cam_photo = await TakeOneShotAsync(index_camera, characteristic);

        if (cam_photo.IsSuccess && cam_photo.Response is not null)
            res.Response = new ShotCameraAsBase64Model(Convert.ToBase64String(cam_photo.Response.Data), cam_photo.Response.Format);

        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<ShotCameraModel>> TakeOneShotAsync(int? index_camera, string? characteristic)
    {
        TResponseModel<ShotCameraModel> res = new();
        string msg;
        IEnumerable<CaptureDeviceDescriptor> devices = new CaptureDevices().EnumerateDescriptors().ToArray();
        if (!devices.Any() || (index_camera is not null && index_camera + 1 > devices.Count()))
        {
            msg = $"Камера с индексом {index_camera} не существует. {(devices.Any() ? $"(доступно:0-{devices.Count() - 1})" : "Камер нет!")}".Trim();
            res.AddError(msg);
            logger.LogError(msg);
            return res;
        }

        CaptureDeviceDescriptor? descriptor0 = index_camera is null
            ? devices.FirstOrDefault()
            : devices.ToArray()[index_camera.Value];

        if (descriptor0 == null)
        {
            msg = $"Could not detect any capture interfaces.";
            res.AddError(msg);
            logger.LogError(msg);
            return res;
        }

        VideoCharacteristics? characteristics = descriptor0.Characteristics
            //.OrderBy(x => x.Width)
            .FirstOrDefault(c => string.IsNullOrWhiteSpace(characteristic) || c.ToString().Equals(characteristic));

        if (characteristics is null)
        {
            msg = $"Could not select primary characteristics.";
            res.AddError(msg);
            logger.LogError(msg);
            return res;
        }

        logger.LogDebug($"Selected capture device: {descriptor0}, {characteristics}");

        byte[] image;
        try
        {
            image = await descriptor0.TakeOneShotAsync(characteristics, new CancellationTokenSource(5000).Token);
        }
        catch (Exception ex)
        {
            res.AddError(ex.Message);
            return res;
        }
        logger.LogDebug($"Captured {image.Length} bytes.");

        string extension = characteristics.PixelFormat switch
        {
            PixelFormats.JPEG => ".jpg",
            PixelFormats.PNG => ".png",   // (Very rare device, I dont know)
            _ => ".bmp",
        };

        res.Response = new ShotCameraModel(image, extension);
        return res;
    }
}