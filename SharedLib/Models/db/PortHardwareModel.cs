////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib
{
    /// <summary>
    /// Порт устройства
    /// </summary>
    public class PortHardwareModel : PortHardwareBaseModel
    {
        /// <summary>
        /// Порт устройства
        /// </summary>
        public PortHardwareModel() { }

        /// <summary>
        /// Порт устройства
        /// </summary>
        public PortHardwareModel(PortModelDB db_port)
        {
            Id = db_port.Id;
            PortNumb = db_port.PortNumb;
            Name = db_port.Name;
            Hardware = new EntryModel() { Id = db_port.Hardware!.Id, Name = db_port.Hardware.Name };
        }

        /// <summary>
        /// Управляющий блок
        /// </summary>
        public EntryModel? Hardware { get; set; }
    }
}