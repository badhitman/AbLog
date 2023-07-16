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

    /// <summary>
    /// Пользователи
    /// </summary>
    public UsersMqttService(IMqttBaseService mqtt)
    {
        _mqtt = mqtt;
    }

    /// <inheritdoc/>
    public async Task<UserResponseModel> GetUser(long telegram_id, CancellationToken cancellation_token = default)
    {
        UserResponseModel res = new();
        SimpleStringResponseModel rpc = await _mqtt.MqttRemoteCall(new LongIdNoiseModel() { Id = telegram_id }, $"{GlobalStatic.Routes.Users}/{GlobalStatic.Routes.GET}", cancellation_token);

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
        /*
         UsersPaginationResponseModel res = new();
        SimpleStringResponseModel rpc = await _mqtt.MqttRemoteCall(new LongIdNoiseModel() { Id = telegram_id }, $"{GlobalStatic.Routes.Users}/{GlobalStatic.Routes.GET}", cancellation_token);

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

        UsersPaginationResponseModel? response_mqtt = JsonConvert.DeserializeObject<UsersPaginationResponseModel>(rpc.Response);

        if (response_mqtt is null)
            res.AddError("response_mqtt is null. error {2309FE2B-4588-4F01-9700-5DC14AA6A4CD}");
        else
            return response_mqtt;

        return res;
         */
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateUser(long telegram_id, UpdateUserModel req, CancellationToken cancellation_token = default)
    {
        throw new NotImplementedException();
    }
}
