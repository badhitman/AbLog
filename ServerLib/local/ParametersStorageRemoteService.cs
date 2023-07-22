////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Newtonsoft.Json;
using SharedLib;

namespace ServerLib;

/// <summary>
/// 
/// </summary>
public class ParametersStorageRemoteService : ParametersStorageLocalService
{
    readonly IMqttBaseService _mqtt;
    readonly MqttConfigModel _mqtt_conf;

    /// <summary>
    /// 
    /// </summary>
    public ParametersStorageRemoteService(IMqttBaseService mqtt_transport, MqttConfigModel mqtt_conf)
    {
        _mqtt = mqtt_transport;
        _mqtt_conf = mqtt_conf;
    }

    /// <inheritdoc/>
    public override async Task<ResponseBaseModel> SaveTelegramBotConfig(TelegramBotConfigModel connect_config)
    {
        SimpleStringResponseModel rpc = await _mqtt.MqttRemoteCall(connect_config, $"{_mqtt_conf.PrefixMqtt}{GlobalStatic.Routes.Storage}/{GlobalStatic.Routes.TelegramBot}/{GlobalStatic.Routes.UPDATE}");
        ResponseBaseModel res = new();
        res.AddMessages(rpc.Messages);
        if (!res.IsSuccess)
            return res;

        if (string.IsNullOrEmpty(rpc.Response))
        {
            res.AddError("string.IsNullOrEmpty(rpc.Response). error {3DD98642-8743-459D-8FC0-9AAC4C180F6F}");
            return res;
        }

        return res;
    }

    /// <inheritdoc/>
    public override async Task<TelegramBotConfigResponseModel> GetTelegramBotConfig()
    {
        SimpleStringResponseModel rpc = await _mqtt.MqttRemoteCall(new NoiseModel(), $"{_mqtt_conf.PrefixMqtt}{GlobalStatic.Routes.Storage}/{GlobalStatic.Routes.TelegramBot}/{GlobalStatic.Routes.GET}");
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

        res = JsonConvert.DeserializeObject<TelegramBotConfigResponseModel>(rpc.Response) ?? new();
        if (res.Conf is null)
        {
            res.Conf = new();
            res.AddError("res.Conf is null error {A0306C7E-7055-4CD2-9143-10923DFD685B}");
        }

        return res;
    }

    /// <inheritdoc/>
    public override async Task<EmailConfigResponseModel> GetEmailConfig()
    {
        SimpleStringResponseModel rpc = await _mqtt.MqttRemoteCall(new NoiseModel(), $"{_mqtt_conf.PrefixMqtt}{GlobalStatic.Routes.Storage}/{GlobalStatic.Routes.Email}/{GlobalStatic.Routes.GET}");
        EmailConfigResponseModel res = new();
        if (!rpc.IsSuccess)
        {
            res.AddMessages(rpc.Messages);
            return res;
        }

        if (string.IsNullOrEmpty(rpc.Response))
        {
            res.AddError("string.IsNullOrEmpty(rpc.Response). error {8B807537-270E-452C-8D6A-C4EF5FAF8DF6}");
            return res;
        }

        res = JsonConvert.DeserializeObject<EmailConfigResponseModel>(rpc.Response) ?? new();
        if (res.Conf is null)
        {
            res.Conf = new();
            res.AddError("res.Conf is null error {EEE8D541-56D2-49F8-BFD2-B543A71BF204}");
        }

        return res;
    }

    /// <inheritdoc/>
    public override async Task<ResponseBaseModel> SaveEmailConfig(EmailConfigModel connect_config)
    {
        SimpleStringResponseModel rpc = await _mqtt.MqttRemoteCall(connect_config, $"{_mqtt_conf.PrefixMqtt}{GlobalStatic.Routes.Storage}/{GlobalStatic.Routes.Email}/{GlobalStatic.Routes.UPDATE}");
        ResponseBaseModel res = new();
        res.AddMessages(rpc.Messages);
        if (!res.IsSuccess)
            return res;

        if (string.IsNullOrEmpty(rpc.Response))
        {
            res.AddError("string.IsNullOrEmpty(rpc.Response). error {8AA0A955-7E07-4689-88D3-14AE2B8FDA64}");
            return res;
        }

        return res;
    }
}