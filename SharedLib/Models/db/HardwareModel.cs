////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib
{
    /// <summary>
    /// Управляющий блок (устройство) - View модель
    /// </summary>
    public class HardwareModel : HardwareBaseModel
    {
        /// <summary>
        /// Управляющий блок (устройство) - View модель
        /// </summary>
        public HardwareModel() { }

        /// <summary>
        /// Управляющий блок (устройство) - View модель
        /// </summary>
        public HardwareModel(HardwareModelDB v)
        {
            Id = v.Id;
            Name = v.Name;
            Address = v.Address;
            AlarmSubscriber = v.AlarmSubscriber;
            CommandsAllowed = v.CommandsAllowed;
            Password = v.Password;
            Ports = v.Ports?.Select(p => new PortHardwareModel(p)).ToList();
        }

        /// <summary>
        /// Порты устройства
        /// </summary>
        public List<PortHardwareModel>? Ports { get; set; }
    }
}