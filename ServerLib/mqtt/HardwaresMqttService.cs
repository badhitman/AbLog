using System.Runtime.Versioning;
using Newtonsoft.Json;
using MQTTnet.Client;
using System.Text;
using SharedLib;
using MQTTnet;

namespace ServerLib;

/// <summary>
/// Устройства
/// </summary>
[SupportedOSPlatform("windows")]
[SupportedOSPlatform("linux")]
[SupportedOSPlatform("android")]
public class HardwaresMqttService : IHardwaresService
{
    readonly MqttFactory _mqttFactory;
    readonly IMqttClient _mqtt_client;
    readonly IMqttBaseService _mqtt;
    readonly MqttConfigModel _conf;

    /// <summary>
    /// Устройства IMqttClient
    /// </summary>
    public HardwaresMqttService(IMqttBaseService mqtt, MqttConfigModel conf, MqttFactory mqttFactory, IMqttClient mqtt_client)
    {
        _mqtt = mqtt;
        _conf = conf;
        _mqttFactory = mqttFactory;
        _mqtt_client = mqtt_client;
    }

    /// <inheritdoc/>
    public async Task<HardwaresResponseModel> HardwaresGetAll(CancellationToken cancellation_token = default)
    {
        HardwaresResponseModel res = new();
        SimpleStringResponseModel rpc = await _mqtt.MqttRemoteCall(new NoiseModel(), $"{GlobalStatic.Routes.Hardwares}/{GlobalStatic.Routes.LIST}", cancellation_token);

        if (!rpc.IsSuccess)
        {
            res.AddError("!rpc.IsSuccess. error {48093688-E3C7-4EDA-92BC-66E0468300A8}");
            return res;
        }

        if (string.IsNullOrEmpty(rpc.Response))
        {
            res.AddError("string.IsNullOrEmpty(rpc.Response). error {DB031034-891C-4C8B-8BDE-8546C6888E71}");
            return res;
        }

        HardwaresResponseModel? response_mqtt = JsonConvert.DeserializeObject<HardwaresResponseModel>(rpc.Response);

        if (response_mqtt is null)
            res.AddError("response_mqtt is null. error {7AB0CF56-0A0F-4CDE-9DFE-F43D3E4C0119}");
        else
            return response_mqtt;

        return res;
    }

    /// <inheritdoc/>
    public async Task<HardwareResponseModel> HardwareGet(int hardware_id, CancellationToken cancellation_token = default)
    {
        HardwareResponseModel res = new();
        if (hardware_id <= 0)
        {
            res.AddError("!rpc.IsSuccess. error {14E0BF67-B692-459D-BAD5-44235C46B36F}");
            return res;
        }

        SimpleStringResponseModel rpc = await _mqtt.MqttRemoteCall(new SimpleIdNoiseModel() { Id = hardware_id }, $"{GlobalStatic.Routes.Hardware}/{GlobalStatic.Routes.GET}", cancellation_token);

        if (!rpc.IsSuccess)
        {
            res.AddError("!rpc.IsSuccess. error {A7C52F1E-AAAF-4426-A9C3-64FD5843B507}");
            return res;
        }

        if (string.IsNullOrEmpty(rpc.Response))
        {
            res.AddError("string.IsNullOrEmpty(rpc.Response). error {C514933D-965B-4AB0-9B77-EE9BD2D145B0}");
            return res;
        }

        HardwareResponseModel? response_mqtt = JsonConvert.DeserializeObject<HardwareResponseModel>(rpc.Response);

        if (response_mqtt is null)
            res.AddError("response_mqtt is null. error {B4E1C721-BA86-43B1-B2CB-ED637134BE61}");
        else
            return response_mqtt;

        return res;
    }

    /// <inheritdoc/>
    public Task<ResponseBaseModel> HardwareDelete(int hardware_id, CancellationToken cancellation_token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<EntriyResponseModel> CheckPortHardware(PortHardwareCheckRequestModel req, CancellationToken cancellation_token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<HttpResponseModel> GetHardwareHtmlPage(HardvareGetRequestModel req, CancellationToken cancellation_token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<PortHardwareResponseModel> HardwarePortGet(int port_id, CancellationToken cancellation_token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<EntriesResponseModel> HardwaresGetAllAsEntries(CancellationToken cancellation_token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<EntriesNestedResponseModel> HardwaresGetTreeNestedEntries(CancellationToken cancellation_token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<HardwareResponseModel> HardwareUpdate(HardwareBaseModel hardware, CancellationToken cancellation_token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<ResponseBaseModel> SetNamePort(EntryModel port_id_name, CancellationToken cancellation_token = default)
    {
        throw new NotImplementedException();
    }
}
/* 
EntriesResponseModel HardwaresGetAllAsEntries();
EntriesNestedResponseModel HardwaresGetTreeNestedEntries();

ResponseBaseModel HardwareDelete(int hardware_id);
HardwareResponseModel HardwareUpdate(HardwareBaseModel hardware);
PortHardwareResponseModel HardwarePortGet(int port_id);
HttpResponseModel GetHardwareHtmlPage(HardvareGetRequestModel req);
EntriyResponseModel CheckPortHardware(PortHardwareCheckRequestModel req);
ResponseBaseModel SetNamePort(EntryModel port_id_name);
 */