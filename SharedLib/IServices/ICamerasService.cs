namespace SharedLib;

/// <summary>
/// FlashCap service
/// </summary>
public interface ICamerasService
{
    /// <summary>
    /// Получить имена камер
    /// </summary>
    public IEnumerable<CameraModel> GetAvailableDevicesNames(string? find_name_camera = null);

    /// <summary>
    /// Получить снимок с камеры (по её индексу).
    /// Если запрос IsNull, то с первой попавшейся камеры.
    /// </summary>
    /// <param name="index_camera">индекс камеры</param>
    /// <param name="characteristic"></param>
    /// <returns>Снимок с запрашиваемой камеры</returns>
    public Task<ShotCameraAsBase64ResponseModel> TakeOneShotAsBase64Async(int? index_camera, string? characteristic);

    /// <summary>
    /// Получить снимок с камеры (по её индексу).
    /// Если запрос IsNull, то с первой попавшейся камеры.
    /// </summary>
    /// <param name="index_camera">индекс камеры</param>
    /// <param name="characteristic"></param>
    /// <returns>Снимок с запрашиваемой камеры</returns>
    public Task<ShotCameraResponseModel> TakeOneShotAsync(int? index_camera, string? characteristic);
}