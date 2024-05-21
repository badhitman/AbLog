////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Запрос/проверка порта устройства
/// </summary>
public class PortHardwareCheckRequestModel
{
    /// <summary>
    /// Hardware Id
    /// </summary>
    public int HardwareId { get; set; }

    /// <summary>
    /// Номер порта
    /// </summary>
    public uint PortNum { get; set; }

    /// <summary>
    /// Создать порт в базе данных (если отсутствует)
    /// </summary>
    public bool CreatePortIfNotExist { get; set; } = true;
}
