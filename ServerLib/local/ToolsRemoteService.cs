////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Newtonsoft.Json;
using SharedLib;
using MQTTnet;

namespace ServerLib;

/// <inheritdoc/>
public class ToolsRemoteService : ToolsLocalService
{

    readonly IMqttBaseService _mqtt;

    /// <inheritdoc/>
    public ToolsRemoteService(IMqttBaseService mqttClientService, IParametersStorageService parameter_storage, MqttFactory mqtt_fact, HttpClient http_client, IMqttBaseService mqtt, IEmailService _email)
        : base(mqttClientService, parameter_storage, mqtt_fact, http_client, _email)
    {
        _mqtt = mqtt;
    }

    /// <inheritdoc/>
    public override async Task<DictionaryResponseModel> TestTelegramBotConnect(TelegramBotConfigModel? conf = null)
    {
        DictionaryResponseModel res = new();
        conf ??= new();

        SimpleStringResponseModel rpc = await _mqtt.MqttRemoteCall(conf, $"{GlobalStatic.Routes.Tools}/{GlobalStatic.Routes.TelegramBot}/{GlobalStatic.Routes.CHECK}");

        if (!rpc.IsSuccess)
        {
            res.AddMessages(rpc.Messages);
            return res;
        }

        if (string.IsNullOrEmpty(rpc.Response))
        {
            res.AddError("string.IsNullOrEmpty(rpc.Response). error {8FC97429-E1D6-4D0F-89B8-F998A1FC098A}");
            return res;
        }

        DictionaryResponseModel? response_mqtt = JsonConvert.DeserializeObject<DictionaryResponseModel>(rpc.Response);

        if (response_mqtt is null)
            res.AddError("response_mqtt is null. error {63B3DB7C-A730-41E9-B3BE-57EFB4F17EB5}");
        else
            return response_mqtt;

        return res;
    }

    /// <inheritdoc/>
    public override async Task<ResponseBaseModel> TestEmailConnect(EmailConfigModel? conf = null)
    {
        ResponseBaseModel res = new();
        conf ??= new();

        SimpleStringResponseModel rpc = await _mqtt.MqttRemoteCall(conf, $"{GlobalStatic.Routes.Tools}/{GlobalStatic.Routes.Email}/{GlobalStatic.Routes.CHECK}");

        if (!rpc.IsSuccess)
        {
            res.AddMessages(rpc.Messages);
            return res;
        }

        if (string.IsNullOrEmpty(rpc.Response))
        {
            res.AddError("string.IsNullOrEmpty(rpc.Response). error {E4ECC37F-9896-42BE-8539-3D893FAA125A}");
            return res;
        }

        ResponseBaseModel? response_mqtt = JsonConvert.DeserializeObject<ResponseBaseModel>(rpc.Response);

        if (response_mqtt is null)
            res.AddError("response_mqtt is null. error {04591427-3537-4687-8199-4EC6EF0A0C7E}");
        else
            return response_mqtt;

        return res;
    }
}