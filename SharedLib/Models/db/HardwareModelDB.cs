////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib
{
    /// <summary>
    /// Управляющий блок (устройство)
    /// </summary>
    public class HardwareModelDB : HardwareBaseModel
    {
        /// <summary>
        /// Порты устройства
        /// </summary>
        public List<PortModelDB>? Ports { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            string hw_name = $"{Name} ({Address})";
            return hw_name;
        }
    }
}