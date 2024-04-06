////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Newtonsoft.Json;
using SharedLib;

namespace ServerLib;

/// <summary>
/// Скрипты
/// </summary>
public class ScriptsMqttService(IMqttBaseService mqtt, MqttConfigModel mqtt_conf) : IScriptsService
{
    /// <inheritdoc/>
    public async Task<ScriptsResponseModel> ScriptDelete(int script_id, CancellationToken cancellation_token = default)
    {
        ScriptsResponseModel res = new();
        SimpleStringResponseModel rpc = await mqtt.MqttRemoteCall(new LongIdNoiseModel() { Id = script_id }, $"{mqtt_conf.PrefixMqtt}{GlobalStatic.Routes.Scripts}/{GlobalStatic.Routes.DELETE}", cancellation_token);

        if (!rpc.IsSuccess)
        {
            res.AddMessages(rpc.Messages);
            return res;
        }
        if (string.IsNullOrEmpty(rpc.TextPayload))
        {
            res.AddError("string.IsNullOrEmpty(rpc.Response). error {17A44C7E-D993-4AD7-807F-AF843678457F}");
            return res;
        }

        ScriptsResponseModel? response_mqtt = JsonConvert.DeserializeObject<ScriptsResponseModel>(rpc.TextPayload);

        if (response_mqtt is null)
            res.AddError("response_mqtt is null. error {34A41A10-5670-44F2-89C0-03A42C6B6662}");
        else
            return response_mqtt;

        return res;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ScriptEnableSet(ObjectStateModel req, CancellationToken cancellation_token = default)
    {
        ResponseBaseModel res = new();
        SimpleStringResponseModel rpc = await mqtt.MqttRemoteCall(req, $"{mqtt_conf.PrefixMqtt}{GlobalStatic.Routes.Scripts}/{GlobalStatic.Routes.ENABLE}", cancellation_token);

        if (!rpc.IsSuccess)
        {
            res.AddMessages(rpc.Messages);
            return res;
        }
        if (string.IsNullOrEmpty(rpc.TextPayload))
        {
            res.AddError("string.IsNullOrEmpty(rpc.Response). error {D4B3ECC5-2140-45FD-82AC-8566EDB4BAF5}");
            return res;
        }
        ResponseBaseModel? response_mqtt = JsonConvert.DeserializeObject<ResponseBaseModel>(rpc.TextPayload);

        if (response_mqtt is null)
            res.AddError("response_mqtt is null. error {258017E3-469B-45D1-A6C9-D7D353233920}");
        else
            return response_mqtt;

        return res;
    }

    /// <inheritdoc/>
    public async Task<ScriptsResponseModel> ScriptsGetAll(CancellationToken cancellation_token = default)
    {
        ScriptsResponseModel res = new();
        SimpleStringResponseModel rpc = await mqtt.MqttRemoteCall(new NoiseModel(), $"{mqtt_conf.PrefixMqtt}{GlobalStatic.Routes.Scripts}/{GlobalStatic.Routes.LIST}", cancellation_token);

        if (!rpc.IsSuccess)
        {
            res.AddMessages(rpc.Messages);
            return res;
        }
        if (string.IsNullOrEmpty(rpc.TextPayload))
        {
            res.AddError("string.IsNullOrEmpty(rpc.Response). error {19A3DFA5-4AEE-448B-A131-F4717D30207E}");
            return res;
        }
        ScriptsResponseModel? response_mqtt = JsonConvert.DeserializeObject<ScriptsResponseModel>(rpc.TextPayload);

        if (response_mqtt is null)
            res.AddError("response_mqtt is null. error {07CD3B30-1D17-409F-BECB-5949792FB0A4}");
        else
            return response_mqtt;

        return res;
    }

    /// <inheritdoc/>
    public async Task<ScriptsResponseModel> ScriptUpdateOrCreate(EntryDescriptionModel script, CancellationToken cancellation_token = default)
    {
        ScriptsResponseModel res = new();
        SimpleStringResponseModel rpc = await mqtt.MqttRemoteCall(script, $"{mqtt_conf.PrefixMqtt}{GlobalStatic.Routes.Scripts}/{GlobalStatic.Routes.UPDATE}", cancellation_token);

        if (!rpc.IsSuccess)
        {
            res.AddMessages(rpc.Messages);
            return res;
        }
        if (string.IsNullOrEmpty(rpc.TextPayload))
        {
            res.AddError("string.IsNullOrEmpty(rpc.Response). error {9396B7D9-8629-4647-9274-3A2D39D1B9F4}");
            return res;
        }
        ScriptsResponseModel? response_mqtt = JsonConvert.DeserializeObject<ScriptsResponseModel>(rpc.TextPayload);

        if (response_mqtt is null)
            res.AddError("response_mqtt is null. error {92DBB463-5AB8-4617-A15B-EC1F289EC141}");
        else
            return response_mqtt;

        return res;
    }
}