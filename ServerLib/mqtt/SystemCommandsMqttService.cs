////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Newtonsoft.Json;
using SharedLib;
using System.Runtime.Versioning;

namespace ServerLib;

/// <summary>
/// Системные команды
/// </summary>
[SupportedOSPlatform("windows")]
[SupportedOSPlatform("linux")]
[SupportedOSPlatform("android")]
public class SystemCommandsMqttService(IMqttBaseService mqtt, MqttConfigModel mqtt_conf) : ISystemCommandsService
{

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> CommandDelete(int comm_id, CancellationToken cancellation_token = default)
    {
        ResponseBaseModel res = new();
        SimpleStringResponseModel rpc = await mqtt.MqttRemoteCall(new SimpleIdNoiseModel() { Id = comm_id }, $"{mqtt_conf.PrefixMqtt}{GlobalStatic.Routes.System}/{GlobalStatic.Routes.Commands}/{GlobalStatic.Routes.DELETE}", cancellation_token);

        if (!rpc.IsSuccess)
        {
            res.AddMessages(rpc.Messages);
            return res;
        }

        if (string.IsNullOrEmpty(rpc.Response))
        {
            res.AddError("string.IsNullOrEmpty(rpc.Response). error {B180864C-A50F-4775-A7BB-7C2B5CDE8F79}");
            return res;
        }

        ResponseBaseModel? response_mqtt = JsonConvert.DeserializeObject<ResponseBaseModel>(rpc.Response);

        if (response_mqtt is null)
            res.AddError("response_mqtt is null. error {32A20B2D-5A7D-4138-96EC-D78B6C5AA19B}");
        else
            return response_mqtt;

        return res;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> CommandRun(int comm_id, CancellationToken cancellation_token = default)
    {
        ResponseBaseModel res = new();
        SimpleStringResponseModel rpc = await mqtt.MqttRemoteCall(new SimpleIdNoiseModel() { Id = comm_id }, $"{mqtt_conf.PrefixMqtt}{GlobalStatic.Routes.System}/{GlobalStatic.Routes.Commands}/{GlobalStatic.Routes.START}", cancellation_token);

        if (!rpc.IsSuccess)
        {
            res.AddMessages(rpc.Messages);
            return res;
        }

        if (string.IsNullOrEmpty(rpc.Response))
        {
            res.AddError("string.IsNullOrEmpty(rpc.Response). error {0766A59A-BF32-4EAD-9971-42F47177148D}");
            return res;
        }

        ResponseBaseModel? response_mqtt = JsonConvert.DeserializeObject<ResponseBaseModel>(rpc.Response);

        if (response_mqtt is null)
            res.AddError("response_mqtt is null. error {4C3B0453-0E06-46EB-81A2-ACBD38283AE5}");
        else
            return response_mqtt;

        return res;
    }

    /// <inheritdoc/>
    public async Task<SystemCommandsResponseModel> CommandsGetAll(CancellationToken cancellation_token = default)
    {
        SystemCommandsResponseModel res = new();
        SimpleStringResponseModel rpc = await mqtt.MqttRemoteCall(new NoiseModel(), $"{mqtt_conf.PrefixMqtt}{GlobalStatic.Routes.System}/{GlobalStatic.Routes.Commands}/{GlobalStatic.Routes.LIST}", cancellation_token);

        if (!rpc.IsSuccess)
        {
            res.AddMessages(rpc.Messages);
            return res;
        }

        if (string.IsNullOrEmpty(rpc.Response))
        {
            res.AddError("string.IsNullOrEmpty(rpc.Response). error {4944F87A-8879-4AE0-BE0A-45FDCE992631}");
            return res;
        }

        SystemCommandsResponseModel? response_mqtt = JsonConvert.DeserializeObject<SystemCommandsResponseModel>(rpc.Response);

        if (response_mqtt is null)
            res.AddError("response_mqtt is null. error {623D7E80-16A4-4DC3-AF64-98D2B1F30106}");
        else
            return response_mqtt;

        return res;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> CommandUpdateOrCreate(SystemCommandModelDB comm, CancellationToken cancellation_token = default)
    {
        ResponseBaseModel res = new();
        SimpleStringResponseModel rpc = await mqtt.MqttRemoteCall(comm, $"{mqtt_conf.PrefixMqtt}{GlobalStatic.Routes.System}/{GlobalStatic.Routes.Commands}/{GlobalStatic.Routes.UPDATE}", cancellation_token);

        if (!rpc.IsSuccess)
        {
            res.AddMessages(rpc.Messages);
            return res;
        }

        if (string.IsNullOrEmpty(rpc.Response))
        {
            res.AddError("string.IsNullOrEmpty(rpc.Response). error {0D199AE5-E697-4563-9F9A-80A1868FEF61}");
            return res;
        }

        ResponseBaseModel? response_mqtt = JsonConvert.DeserializeObject<ResponseBaseModel>(rpc.Response);

        if (response_mqtt is null)
            res.AddError("response_mqtt is null. error {618B75ED-78A2-4A34-9DAC-DB899FC193AF}");
        else
            return response_mqtt;

        return res;
    }
}