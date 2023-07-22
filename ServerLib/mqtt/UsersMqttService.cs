////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Newtonsoft.Json;
using SharedLib;

namespace ServerLib;

/// <summary>
/// Пользователи
/// </summary>
public class UsersMqttService : IUsersService
{
    readonly IMqttBaseService _mqtt;
    readonly MqttConfigModel _mqtt_conf;

    /// <summary>
    /// Пользователи
    /// </summary>
    public UsersMqttService(IMqttBaseService mqtt, MqttConfigModel mqtt_conf)
    {
        _mqtt = mqtt;
        _mqtt_conf = mqtt_conf;
    }

    /// <inheritdoc/>
    public async Task<UserResponseModel> GetUser(long telegram_id, CancellationToken cancellation_token = default)
    {
        UserResponseModel res = new();
        SimpleStringResponseModel rpc = await _mqtt.MqttRemoteCall(new LongIdNoiseModel() { Id = telegram_id }, $"{_mqtt_conf.PrefixMqtt}{GlobalStatic.Routes.Users}/{GlobalStatic.Routes.GET}", cancellation_token);

        if (!rpc.IsSuccess)
        {
            res.AddMessages(rpc.Messages);
            return res;
        }

        if (string.IsNullOrEmpty(rpc.Response))
        {
            res.AddError("string.IsNullOrEmpty(rpc.Response). error {AD9E54A0-00BD-48AB-B368-801EE542220D}");
            return res;
        }

        UserResponseModel? response_mqtt = JsonConvert.DeserializeObject<UserResponseModel>(rpc.Response);

        if (response_mqtt is null)
            res.AddError("response_mqtt is null. error {2309FE2B-4588-4F01-9700-5DC14AA6A4CD}");
        else
            return response_mqtt;

        return res;
    }

    /// <inheritdoc/>
    public async Task<UsersPaginationResponseModel> UsersGetList(UserListGetModel req, CancellationToken cancellation_token = default)
    {
        UsersPaginationResponseModel res = new();
        SimpleStringResponseModel rpc = await _mqtt.MqttRemoteCall(req, $"{_mqtt_conf.PrefixMqtt}{GlobalStatic.Routes.Users}/{GlobalStatic.Routes.GET}/{GlobalStatic.Routes.LIST}", cancellation_token);

        if (!rpc.IsSuccess)
        {
            res.AddMessages(rpc.Messages);
            return res;
        }

        if (string.IsNullOrEmpty(rpc.Response))
        {
            res.AddError("string.IsNullOrEmpty(rpc.Response). error {745E7C66-EEEF-40A7-B9A9-B156F1E61A5D}");
            return res;
        }

        UsersPaginationResponseModel? response_mqtt = JsonConvert.DeserializeObject<UsersPaginationResponseModel>(rpc.Response);

        if (response_mqtt is null)
            res.AddError("response_mqtt is null. error {68703F46-E066-4613-B3A8-87880E7E6489}");
        else
            return response_mqtt;

        return res;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateUser(UpdateUserModel req, CancellationToken cancellation_token = default)
    {
        ResponseBaseModel res = new();
        SimpleStringResponseModel rpc = await _mqtt.MqttRemoteCall(req, $"{_mqtt_conf.PrefixMqtt}{GlobalStatic.Routes.Users}/{GlobalStatic.Routes.UPDATE}", cancellation_token);

        if (!rpc.IsSuccess)
        {
            res.AddMessages(rpc.Messages);
            return res;
        }

        if (string.IsNullOrEmpty(rpc.Response))
        {
            res.AddError("string.IsNullOrEmpty(rpc.Response). error {884CA216-067A-4AF1-B730-A7C93D45FA89}");
            return res;
        }

        ResponseBaseModel? response_mqtt = JsonConvert.DeserializeObject<ResponseBaseModel>(rpc.Response);

        if (response_mqtt is null)
            res.AddError("response_mqtt is null. error {D80AC4A3-5974-4F39-AE0D-E2065A4500D8}");
        else
            return response_mqtt;

        return res;
    }
}