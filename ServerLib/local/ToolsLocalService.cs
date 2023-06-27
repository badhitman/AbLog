using MQTTnet.Client;
using SharedLib;
using MQTTnet;

namespace ServerLib;

/// <summary>
/// 
/// </summary>
public class ToolsLocalService : IToolsService
{
    readonly IParametersStorageService _parameter_storage;
    readonly IMqttBaseService _mqttClientService;
    readonly MqttFactory _mqtt_fact;

    /// <summary>
    /// 
    /// </summary>
    public ToolsLocalService(IMqttBaseService mqttClientService, IParametersStorageService parameter_storage, MqttFactory mqtt_fact)
    {
        _mqttClientService = mqttClientService;
        _parameter_storage = parameter_storage;
        _mqtt_fact = mqtt_fact;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> StartMqtt() => await _mqttClientService.StartService();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> StopMqtt() => await _mqttClientService.StopService();

    /// <inheritdoc/>
    public Task<BoolResponseModel> StatusMqtt() => Task.FromResult(_mqttClientService.StatusService());

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> TestEmailConnect(EmailConfigModel? conf = null)
    {
        ResponseBaseModel res = new();
        conf ??= (await _parameter_storage.GetEmailConfig()).Conf;
        if (!conf!.IsConfigured)
        {
            res.AddError("Конфигурация не установлена");
            return res;
        }

        using EmailLocalService emailService = new();
        res = await emailService.ConnectSmtpAsync(conf);
        return res;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> TestMqttConnect(MqttConfigModel? conf = null)
    {
        ResponseBaseModel res = new();
        conf ??= (await _parameter_storage.GetMqttConfig()).Conf;
        if (!conf!.IsConfigured)
        {
            res.AddError("Конфигурация не установлена");
            return res;
        }

        MqttClientOptions mqttClientOptions = new MqttClientOptionsBuilder()
           .WithTls()
           .WithClientId(conf.ClientId)
           .WithTcpServer(conf.Server, conf.Port)
           .WithCredentials(conf.Username, conf.Password)
           .WithCleanSession()
        .Build();
        using IMqttClient _mqtt = _mqtt_fact.CreateMqttClient();
        try
        {
            MqttClientConnectResult connect_res = await _mqtt.ConnectAsync(mqttClientOptions, CancellationToken.None);

            if (connect_res.ResultCode != MqttClientConnectResultCode.Success)
                res.AddError(connect_res.ResultCode.ToString());
            else
            {
                res.AddSuccess($"Подключение успешно: {connect_res.ResultCode}");
                await _mqtt.DisconnectAsync();
            }
        }
        catch (Exception ex)
        {
            res.AddError($"Failed to connect {ex.Message}");
        }

        return res;
    }
}