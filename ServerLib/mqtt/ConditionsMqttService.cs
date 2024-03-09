////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Newtonsoft.Json;
using SharedLib;

namespace ServerLib;

/// <summary>
/// Ограничения
/// </summary>
public class ConditionsMqttService(IMqttBaseService mqtt, MqttConfigModel mqtt_conf) : IConditionsService
{
    /// <inheritdoc/>
    public async Task<ConditionsAnonimResponseModel> ConditionDelete(int condition_id, ConditionsTypesEnum condition_type, CancellationToken cancellation_token = default)
    {
        ConditionsAnonimResponseModel res = new();
        SimpleStringResponseModel rpc = await mqtt.MqttRemoteCall(new LongIdNoiseModel() { Id = condition_id }, $"{mqtt_conf.PrefixMqtt}{GlobalStatic.Routes.Conditions}/{GlobalStatic.Routes.DELETE}", cancellation_token);

        if (!rpc.IsSuccess)
        {
            res.AddMessages(rpc.Messages);
            return res;
        }
        if (string.IsNullOrEmpty(rpc.Response))
        {
            res.AddError("string.IsNullOrEmpty(rpc.Response). error {61E678BA-D547-4C96-83DA-471F5F1468D6}");
            return res;
        }
        ConditionsAnonimResponseModel? response_mqtt = JsonConvert.DeserializeObject<ConditionsAnonimResponseModel>(rpc.Response);

        if (response_mqtt is null)
            res.AddError("response_mqtt is null. error {EEF2F5BC-7678-45C7-B190-F20558F746AA}");
        else
            return response_mqtt;

        return res;
    }

    /// <inheritdoc/>
    public async Task<ConditionsAnonimResponseModel> ConditionsGetByOwner(int owner_id, ConditionsTypesEnum condition_type, CancellationToken cancellation_token = default)
    {
        ConditionsAnonimResponseModel res = new();
        SimpleStringResponseModel rpc = await mqtt.MqttRemoteCall(new LongIdNoiseModel() { Id = owner_id }, $"{mqtt_conf.PrefixMqtt}{GlobalStatic.Routes.Conditions}/{GlobalStatic.Routes.BY_OWNER}", cancellation_token);

        if (!rpc.IsSuccess)
        {
            res.AddMessages(rpc.Messages);
            return res;
        }
        if (string.IsNullOrEmpty(rpc.Response))
        {
            res.AddError("string.IsNullOrEmpty(rpc.Response). error {76D0FFFA-F0BA-4995-8912-1EE3C0831BC0}");
            return res;
        }
        ConditionsAnonimResponseModel? response_mqtt = JsonConvert.DeserializeObject<ConditionsAnonimResponseModel>(rpc.Response);

        if (response_mqtt is null)
            res.AddError("response_mqtt is null. error {355C71F3-B5B8-4C65-A74D-DD5838DC1324}");
        else
            return response_mqtt;

        return res;
    }

    /// <inheritdoc/>
    public async Task<ConditionsAnonimResponseModel> ConditionUpdateOrCreate(ConditionUpdateModel req, CancellationToken cancellation_token = default)
    {
        ConditionsAnonimResponseModel res = new();
        SimpleStringResponseModel rpc = await mqtt.MqttRemoteCall(req, $"{mqtt_conf.PrefixMqtt}{GlobalStatic.Routes.Conditions}/{GlobalStatic.Routes.UPDATE}", cancellation_token);

        if (!rpc.IsSuccess)
        {
            res.AddMessages(rpc.Messages);
            return res;
        }
        if (string.IsNullOrEmpty(rpc.Response))
        {
            res.AddError("string.IsNullOrEmpty(rpc.Response). error {68613B5C-681F-4B5D-A532-2D48F65D5FBE}");
            return res;
        }
        ConditionsAnonimResponseModel? response_mqtt = JsonConvert.DeserializeObject<ConditionsAnonimResponseModel>(rpc.Response);

        if (response_mqtt is null)
            res.AddError("response_mqtt is null. error {5BE9431F-C504-4C2C-A8CA-F232829BBA90}");
        else
            return response_mqtt;

        return res;
    }
}