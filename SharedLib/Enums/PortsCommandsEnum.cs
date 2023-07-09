////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Команды порта: вкл, выкл или переключение в противоположное состояние
/// </summary>
public enum PortsCommandsEnum
{
    /// <summary>
    /// Переключение в противоположное состояние
    /// </summary>
    Switching = 0,

    /// <summary>
    /// Вкл
    /// </summary>
    On = 1,

    /// <summary>
    /// Выкл
    /// </summary>
    Off = 2,
}
