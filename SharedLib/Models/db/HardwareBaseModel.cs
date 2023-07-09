////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// Управляющий блок (устройство) - Базовая модель
/// </summary>
[Index(nameof(Address), IsUnique = true)]
public class HardwareBaseModel : UniversalModelDB
{
    /// <summary>
    /// Управляющий блок (устройство) - Базовая модель
    /// </summary>
    public HardwareBaseModel() { }
    /// <summary>
    /// Управляющий блок (устройство) - Базовая модель
    /// </summary>
    public HardwareBaseModel(HardwareModelDB v)
    {
        Id = v.Id;
        Name = v.Name;
        Address = v.Address;
        AlarmSubscriber = v.AlarmSubscriber;
        CommandsAllowed = v.CommandsAllowed;
        Password = v.Password;
    }

    /// <summary>
    /// Сетевой адрес (заводской: 192.168.0.14)
    /// </summary>
    [Required(ErrorMessage = "Адрес обязателен для заполнения")]
    public string Address { get; set; } = default!;

    /// <summary>
    /// Пароль (заводской: sec)
    /// </summary>
    [Required(ErrorMessage = "Заполните пароль доступа к контроллеру")]
    public string Password { get; set; } = default!;
}