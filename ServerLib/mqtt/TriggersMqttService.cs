////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Newtonsoft.Json;
using SharedLib;

namespace ServerLib;

/// <summary>
/// Триггеры событий
/// </summary>
public class TriggersMqttService(IMqttBaseService mqtt, MqttConfigModel mqtt_conf) : ITriggersService
{
    /// <inheritdoc/>
    public async Task<TResponseModel<List<TrigerModelDB>>> TriggerDelete(int trigger_id, CancellationToken cancellation_token = default)
    {
        TResponseModel<List<TrigerModelDB>> res = new();
        TResponseModel<string> rpc = await mqtt.MqttRemoteCall(new LongIdNoiseModel() { Id = trigger_id }, $"{mqtt_conf.PrefixMqtt}{GlobalStatic.Routes.Triggers}/{GlobalStatic.Routes.DELETE}", cancellation_token);

        if (!rpc.IsSuccess)
        {
            res.AddMessages(rpc.Messages);
            return res;
        }
        if (string.IsNullOrEmpty(rpc.Response))
        {
            res.AddError("string.IsNullOrEmpty(rpc.Response). error {EF39DBD4-8916-4D90-957F-120815B198B0}");
            return res;
        }
        TResponseModel<List<TrigerModelDB>>? response_mqtt = JsonConvert.DeserializeObject<TResponseModel<List<TrigerModelDB>>>(rpc.Response);

        if (response_mqtt is null)
            res.AddError("response_mqtt is null. error {05692E9A-912E-4EBD-AB34-B7B8A0DC87AD}");
        else
            return response_mqtt;

        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<List<TrigerModelDB>>> TriggersGetAll(CancellationToken cancellation_token = default)
    {
        TResponseModel<List<TrigerModelDB>> res = new();
        TResponseModel<string> rpc = await mqtt.MqttRemoteCall(new NoiseModel() { }, $"{mqtt_conf.PrefixMqtt}{GlobalStatic.Routes.Triggers}/{GlobalStatic.Routes.LIST}", cancellation_token);

        if (!rpc.IsSuccess)
        {
            res.AddMessages(rpc.Messages);
            return res;
        }
        if (string.IsNullOrEmpty(rpc.Response))
        {
            res.AddError("string.IsNullOrEmpty(rpc.Response). error {7A911EF6-7F07-4CC8-9EA7-419DFB03D02C}");
            return res;
        }
        TResponseModel<List<TrigerModelDB>>? response_mqtt = JsonConvert.DeserializeObject<TResponseModel<List<TrigerModelDB>>>(rpc.Response);

        if (response_mqtt is null)
            res.AddError("response_mqtt is null. error {2F64B22E-49B0-4E2E-AC13-036D51FF58AB}");
        else
            return response_mqtt;

        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<List<TrigerModelDB>>> TriggerUpdateOrCreate(TrigerModelDB req, CancellationToken cancellation_token = default)
    {
        TResponseModel<List<TrigerModelDB>> res = new();
        TResponseModel<string> rpc = await mqtt.MqttRemoteCall(req, $"{mqtt_conf.PrefixMqtt}{GlobalStatic.Routes.Triggers}/{GlobalStatic.Routes.UPDATE}", cancellation_token);

        if (!rpc.IsSuccess)
        {
            res.AddMessages(rpc.Messages);
            return res;
        }

        if (string.IsNullOrEmpty(rpc.Response))
        {
            res.AddError("string.IsNullOrEmpty(rpc.Response). error {3E3C8000-FA89-4E87-9FA1-648B5046ABDF}");
            return res;
        }

        TResponseModel<List<TrigerModelDB>>? response_mqtt = JsonConvert.DeserializeObject<TResponseModel<List<TrigerModelDB>>>(rpc.Response);

        if (response_mqtt is null)
            res.AddError("response_mqtt is null. error {9E793982-783B-4897-A24E-5B9F7FA43C6B}");
        else
            return response_mqtt;

        return res;
    }
}