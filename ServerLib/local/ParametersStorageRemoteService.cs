////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Newtonsoft.Json;
using SharedLib;
using System.Diagnostics;

namespace ServerLib;

/// <summary>
/// 
/// </summary>
public class ParametersStorageRemoteService : ParametersStorageLocalService
{
    IMqttBaseService _mqtt;
    /// <summary>
    /// 
    /// </summary>
    public ParametersStorageRemoteService(IMqttBaseService mqtt_transport)
    {
        _mqtt = mqtt_transport;
    }

    /// <inheritdoc/>
    public override Task<ResponseBaseModel> SaveTelegramBotConfig(TelegramBotConfigModel connect_config)
    {


        return base.SaveTelegramBotConfig(connect_config);
    }

    /// <inheritdoc/>
    public override async Task<TelegramBotConfigResponseModel> GetTelegramBotConfig()
    {
        SimpleStringResponseModel rpc = await _mqtt.MqttRemoteCall(new NoiseModel(), $"{GlobalStatic.Routes.Storage}/{GlobalStatic.Routes.TelegramBot}/{GlobalStatic.Routes.GET}");
        TelegramBotConfigResponseModel res = new();
        if (!rpc.IsSuccess)
        {
            res.AddMessages(rpc.Messages);
            return res;
        }

        if (string.IsNullOrEmpty(rpc.Response))
        {
            res.AddError("string.IsNullOrEmpty(rpc.Response). error {3DD98642-8743-459D-8FC0-9AAC4C180F6F}");
            return res;
        }

        res.Conf = JsonConvert.DeserializeObject<TelegramBotConfigModel>(rpc.Response);

        if (res.Conf is null)
            res.AddError("response_mqtt is null. error {B36B8278-ADF1-4982-B496-5AF6A2169199}");

        return res;
    }
}