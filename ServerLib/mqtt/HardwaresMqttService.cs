////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Newtonsoft.Json;
using SharedLib;
using System.Runtime.Versioning;

namespace ServerLib;

/// <summary>
/// Устройства
/// </summary>
/// <remarks>
/// Устройства IMqttClient
/// </remarks>
[SupportedOSPlatform("windows")]
[SupportedOSPlatform("linux")]
[SupportedOSPlatform("android")]
public class HardwaresMqttService(IMqttBaseService mqtt, MqttConfigModel mqtt_conf) : IHardwaresService
{

    /// <inheritdoc/>
    public async Task<HardwaresResponseModel> HardwaresGetAll(CancellationToken cancellation_token = default)
    {
        HardwaresResponseModel res = new();
        SimpleStringResponseModel rpc = await mqtt.MqttRemoteCall(new NoiseModel(), $"{mqtt_conf.PrefixMqtt}{GlobalStatic.Routes.Hardwares}/{GlobalStatic.Routes.LIST}", cancellation_token);

        if (!rpc.IsSuccess)
        {
            res.AddMessages(rpc.Messages);
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
    public async Task<HttpResponseModel> GetHardwareHtmlPage(HardwareGetHttpRequestModel req, CancellationToken cancellation_token = default)
    {

        HttpResponseModel res = new();
        if (req.HardwareId <= 0)
        {
            res.AddError("req.HardwareId <= 0. error {CEBF08AE-721E-4CC8-A2A5-CFE980316F3C}");
            return res;
        }

        SimpleStringResponseModel rpc = await mqtt.MqttRemoteCall(req, $"{mqtt_conf.PrefixMqtt}{GlobalStatic.Routes.Hardwares}/{GlobalStatic.Routes.HTTP}", cancellation_token);

        if (!rpc.IsSuccess)
        {
            res.AddMessages(rpc.Messages);
            return res;
        }

        if (string.IsNullOrEmpty(rpc.Response))
        {
            res.AddError("string.IsNullOrEmpty(rpc.Response). error {D06F8DE6-4189-49E3-907A-2996823C1A87}");
            return res;
        }

        HttpResponseModel? response_mqtt = JsonConvert.DeserializeObject<HttpResponseModel>(rpc.Response);

        if (response_mqtt is null)
            res.AddError("response_mqtt is null. error {F868C3D5-1761-4965-8120-9B42B9281521}");
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

        SimpleStringResponseModel rpc = await mqtt.MqttRemoteCall(new SimpleIdNoiseModel() { Id = hardware_id }, $"{mqtt_conf.PrefixMqtt}{GlobalStatic.Routes.Hardware}/{GlobalStatic.Routes.GET}", cancellation_token);

        if (!rpc.IsSuccess)
        {
            res.AddMessages(rpc.Messages);
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
    public async Task<EntriyResponseModel> CheckPortHardware(PortHardwareCheckRequestModel req, CancellationToken cancellation_token = default)
    {
        EntriyResponseModel res = new();
        if (req.HardwareId <= 0)
        {
            res.AddError("req.HardwareId <= 0. error {6E82E9D2-881D-4811-A4EB-90C9C57D6B3E}");
            return res;
        }

        SimpleStringResponseModel rpc = await mqtt.MqttRemoteCall(req, $"{mqtt_conf.PrefixMqtt}{GlobalStatic.Routes.Port}/{GlobalStatic.Routes.CHECK}", cancellation_token);

        if (!rpc.IsSuccess)
        {
            res.AddMessages(rpc.Messages);
            return res;
        }

        if (string.IsNullOrEmpty(rpc.Response))
        {
            res.AddError("string.IsNullOrEmpty(rpc.Response). error {8A686A51-710F-41DD-A0F5-43891548A1E6}");
            return res;
        }

        EntriyResponseModel? response_mqtt = JsonConvert.DeserializeObject<EntriyResponseModel>(rpc.Response);

        if (response_mqtt is null)
            res.AddError("response_mqtt is null. error {B27011ED-F460-47A2-9BBC-F2551AF66FE8}");
        else
            return response_mqtt;

        return res;
    }

    /// <inheritdoc/>
    public async Task<PortHardwareResponseModel> HardwarePortGet(int port_id, CancellationToken cancellation_token = default)
    {
        PortHardwareResponseModel res = new();
        if (port_id <= 0)
        {
            res.AddError("port_id <= 0. error {AD191D14-A157-432C-A27F-EA145736C771}");
            return res;
        }

        SimpleStringResponseModel rpc = await mqtt.MqttRemoteCall(new SimpleIdNoiseModel() { Id = port_id }, $"{mqtt_conf.PrefixMqtt}{GlobalStatic.Routes.Port}/{GlobalStatic.Routes.GET}", cancellation_token);

        if (!rpc.IsSuccess)
        {
            res.AddMessages(rpc.Messages);
            return res;
        }

        if (string.IsNullOrEmpty(rpc.Response))
        {
            res.AddError("string.IsNullOrEmpty(rpc.Response). error {0F024FA9-D04A-4625-BCF2-DF94C1794495}");
            return res;
        }

        PortHardwareResponseModel? response_mqtt = JsonConvert.DeserializeObject<PortHardwareResponseModel>(rpc.Response);

        if (response_mqtt is null)
            res.AddError("response_mqtt is null. error {0CC2267F-5F91-4064-95B7-671701B71860}");
        else
            return response_mqtt;

        return res;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetNamePort(EntryModel port_id_name, CancellationToken cancellation_token = default)
    {
        ResponseBaseModel res = new();
        if (port_id_name.Id <= 0)
        {
            res.AddError("port_id_name.Id <= 0. error {C27B6E42-455D-4CDC-B31B-3A4CD874B16F}");
            return res;
        }

        SimpleStringResponseModel rpc = await mqtt.MqttRemoteCall(port_id_name, $"{mqtt_conf.PrefixMqtt}{GlobalStatic.Routes.Port}/{GlobalStatic.Routes.UPDATE}", cancellation_token);

        if (!rpc.IsSuccess)
        {
            res.AddMessages(rpc.Messages);
            return res;
        }

        if (string.IsNullOrEmpty(rpc.Response))
        {
            res.AddError("string.IsNullOrEmpty(rpc.Response). error {BE75698E-7956-48AA-A56E-85EFD75FDC63}");
            return res;
        }

        ResponseBaseModel? response_mqtt = JsonConvert.DeserializeObject<ResponseBaseModel>(rpc.Response);

        if (response_mqtt is null)
            res.AddError("response_mqtt is null. error {DFA6BCC6-ACC5-48DC-95CE-2549CD45431B}");
        else
            return response_mqtt;

        return res;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> HardwareDelete(int hardware_id, CancellationToken cancellation_token = default)
    {
        ResponseBaseModel res = new();
        if (hardware_id <= 0)
        {
            res.AddError("hardware_id <= 0. error {6D17235A-CA33-492B-95B7-A2C42E4B22F2}");
            return res;
        }

        SimpleStringResponseModel rpc = await mqtt.MqttRemoteCall(new SimpleIdNoiseModel() { Id = hardware_id }, $"{mqtt_conf.PrefixMqtt}{GlobalStatic.Routes.Hardware}/{GlobalStatic.Routes.DELETE}", cancellation_token);

        if (!rpc.IsSuccess)
        {
            res.AddMessages(rpc.Messages);
            return res;
        }

        if (string.IsNullOrEmpty(rpc.Response))
        {
            res.AddError("string.IsNullOrEmpty(rpc.Response). error {9EC93B0A-13E5-4063-877B-0BAA59A23956}");
            return res;
        }

        ResponseBaseModel? response_mqtt = JsonConvert.DeserializeObject<ResponseBaseModel>(rpc.Response);

        if (response_mqtt is null)
            res.AddError("response_mqtt is null. error {00FCF155-060F-4F5D-915A-0356BA3F3F59}");
        else
            return response_mqtt;

        return res;
    }

    /// <inheritdoc/>
    public async Task<EntriesResponseModel> HardwaresGetAllAsEntries(CancellationToken cancellation_token = default)
    {
        EntriesResponseModel res = new();

        SimpleStringResponseModel rpc = await mqtt.MqttRemoteCall(new NoiseModel(), $"{mqtt_conf.PrefixMqtt}{GlobalStatic.Routes.Hardware}/{GlobalStatic.Routes.ENTRIES}", cancellation_token);

        if (!rpc.IsSuccess)
        {
            res.AddMessages(rpc.Messages);
            return res;
        }

        if (string.IsNullOrEmpty(rpc.Response))
        {
            res.AddError("string.IsNullOrEmpty(rpc.Response). error {40220344-F391-4C8B-B3A9-4542745E3320}");
            return res;
        }

        EntriesResponseModel? response_mqtt = JsonConvert.DeserializeObject<EntriesResponseModel>(rpc.Response);

        if (response_mqtt is null)
            res.AddError("response_mqtt is null. error {937229AF-CD9E-48FF-A6F4-9641B12A73F9}");
        else
            return response_mqtt;

        return res;
    }

    /// <inheritdoc/>
    public async Task<EntriesNestedResponseModel> HardwaresGetTreeNestedEntries(CancellationToken cancellation_token = default)
    {
        EntriesNestedResponseModel res = new();

        SimpleStringResponseModel rpc = await mqtt.MqttRemoteCall(new NoiseModel(), $"{mqtt_conf.PrefixMqtt}{GlobalStatic.Routes.Hardware}/{GlobalStatic.Routes.NESTED_ENTRIES}", cancellation_token);

        if (!rpc.IsSuccess)
        {
            res.AddMessages(rpc.Messages);
            return res;
        }

        if (string.IsNullOrEmpty(rpc.Response))
        {
            res.AddError("string.IsNullOrEmpty(rpc.Response). error {C403AF02-CB86-4666-BEC2-39CA7BF3796C}");
            return res;
        }

        EntriesNestedResponseModel? response_mqtt = JsonConvert.DeserializeObject<EntriesNestedResponseModel>(rpc.Response);

        if (response_mqtt is null)
            res.AddError("response_mqtt is null. error {234721F9-4B3F-428D-BCE0-D763615018A0}");
        else
            return response_mqtt;

        return res;
    }

    /// <inheritdoc/>
    public async Task<HardwareResponseModel> HardwareUpdate(HardwareBaseModel hardware, CancellationToken cancellation_token = default)
    {
        HardwareResponseModel res = new();

        SimpleStringResponseModel rpc = await mqtt.MqttRemoteCall(hardware, $"{mqtt_conf.PrefixMqtt}{GlobalStatic.Routes.Hardware}/{GlobalStatic.Routes.UPDATE}", cancellation_token);

        if (!rpc.IsSuccess)
        {
            res.AddMessages(rpc.Messages);
            return res;
        }

        if (string.IsNullOrEmpty(rpc.Response))
        {
            res.AddError("string.IsNullOrEmpty(rpc.Response). error {496EF23C-8190-43BD-A300-F4DB21B2F5D8}");
            return res;
        }

        HardwareResponseModel? response_mqtt = JsonConvert.DeserializeObject<HardwareResponseModel>(rpc.Response);

        if (response_mqtt is null)
            res.AddError("response_mqtt is null. error {6E5FD7AB-300D-40E9-A5AC-C6E386C571F6}");
        else
            return response_mqtt;

        return res;
    }
}