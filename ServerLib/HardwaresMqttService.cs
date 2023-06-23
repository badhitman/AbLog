using MQTTnet.Client;
using SharedLib;

namespace ServicesLib
{
    /// <summary>
    /// Устройства
    /// </summary>
    public class HardwaresMqttService : IHardwaresService
    {
        readonly IMqttClient _mqtt;

        /// <summary>
        /// Устройства IMqttClient
        /// </summary>
        public HardwaresMqttService(IMqttClient mqtt)
        {
            _mqtt = mqtt;
        }

        /// <inheritdoc/>
        public Task<HardwaresResponseModel> HardwaresGetAll()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<EntriyResponseModel> CheckPortHardware(PortHardwareCheckRequestModel req)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<HttpResponseModel> GetHardwareHtmlPage(HardvareGetRequestModel req)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<ResponseBaseModel> HardwareDelete(int hardware_id)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<HardwareResponseModel> HardwareGet(int hardware_id)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<PortHardwareResponseModel> HardwarePortGet(int port_id)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<EntriesResponseModel> HardwaresGetAllAsEntries()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<EntriesNestedResponseModel> HardwaresGetTreeNestedEntries()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<HardwareResponseModel> HardwareUpdate(HardwareBaseModel hardware)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<ResponseBaseModel> SetNamePort(EntryModel port_id_name)
        {
            throw new NotImplementedException();
        }
    }
}