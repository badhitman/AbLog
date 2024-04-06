////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Newtonsoft.Json;
using SharedLib;

namespace ServerLib;

/// <summary>
/// Конкуренция
/// </summary>
public class ContentionsMqttService(IMqttBaseService mqtt, MqttConfigModel mqtt_conf) : IContentionsService
{
    /// <inheritdoc/>
    public async Task<IdsResponseModel> ContentionSet(ContentionUpdateModel req, CancellationToken cancellation_token = default)
    {
        IdsResponseModel res = new();
        SimpleStringResponseModel rpc = await mqtt.MqttRemoteCall(req, $"{mqtt_conf.PrefixMqtt}{GlobalStatic.Routes.Contentions}/{GlobalStatic.Routes.UPDATE}", cancellation_token);

        if (!rpc.IsSuccess)
        {
            res.AddMessages(rpc.Messages);
            return res;
        }
        if (string.IsNullOrEmpty(rpc.TextPayload))
        {
            res.AddError("string.IsNullOrEmpty(rpc.Response). error {45DDFB65-2075-4364-8290-EB60AD729F7A}");
            return res;
        }
        IdsResponseModel? response_mqtt = JsonConvert.DeserializeObject<IdsResponseModel>(rpc.TextPayload);

        if (response_mqtt is null)
            res.AddError("response_mqtt is null. error {755DEB39-3AEB-4140-92B2-40F38782F937}");
        else
            return response_mqtt;

        return res;
    }

    /// <inheritdoc/>
    public async Task<IdsResponseModel> ContentionsGetByScript(int script_id, CancellationToken cancellation_token = default)
    {
        IdsResponseModel res = new();
        SimpleStringResponseModel rpc = await mqtt.MqttRemoteCall(new LongIdNoiseModel() { Id = script_id }, $"{mqtt_conf.PrefixMqtt}{GlobalStatic.Routes.Contentions}/{GlobalStatic.Routes.BY_OWNER}-{GlobalStatic.Routes.Scripts}", cancellation_token);

        if (!rpc.IsSuccess)
        {
            res.AddMessages(rpc.Messages);
            return res;
        }
        if (string.IsNullOrEmpty(rpc.TextPayload))
        {
            res.AddError("string.IsNullOrEmpty(rpc.Response). error {0434B2F8-C172-4C18-B60E-E94411191441}");
            return res;
        }
        IdsResponseModel? response_mqtt = JsonConvert.DeserializeObject<IdsResponseModel>(rpc.TextPayload);

        if (response_mqtt is null)
            res.AddError("response_mqtt is null. error {A1A7A5E7-A592-49D4-9D21-1509C3E117F0}");
        else
            return response_mqtt;

        return res;
    }
}