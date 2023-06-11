////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib
{
    /// <summary>
    /// Управляющий блок (устройство) - Базовая модель
    /// </summary>
    [Index(nameof(Address), IsUnique = true)]
    public abstract class HardwareBaseModel : UniversalModelDB
    {
        /// <summary>
        /// Сетевой адрес (заводской: 192.168.0.14)
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Пароль (заводской: sec)
        /// </summary>
        public string? Password { get; set; }
    }
}