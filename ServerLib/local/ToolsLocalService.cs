////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.Extensions.DependencyInjection;
using MQTTnet;
using MQTTnet.Adapter;
using MQTTnet.Client;
using SharedLib;
using Telegram.Bot;

namespace ServerLib;

/// <summary>
/// 
/// </summary>
public class ToolsLocalService : IToolsService
{
    readonly IParametersStorageService _parameter_storage;
    readonly IMqttBaseService _mqttClientService;
    readonly MqttFactory _mqtt_fact;
    readonly HttpClient _http_client;
    readonly IEmailService _email;
    readonly IServiceProvider _service_provider;

    /// <summary>
    /// 
    /// </summary>
    public ToolsLocalService(IMqttBaseService mqttClientService, IParametersStorageService parameter_storage, MqttFactory mqtt_fact, HttpClient http_client, IEmailService email, IServiceProvider service_provider)
    {
        _mqttClientService = mqttClientService;
        _parameter_storage = parameter_storage;
        _mqtt_fact = mqtt_fact;
        _http_client = http_client;
        _email = email;
        _service_provider = service_provider;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> StartMqtt() => await _mqttClientService.StartService();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> StopMqtt() => await _mqttClientService.StopService();

    /// <inheritdoc/>
    public Task<BoolResponseModel> StatusMqtt() => _mqttClientService.StatusService();

    /// <inheritdoc/>
    public virtual async Task<ResponseBaseModel> TestEmailConnect(EmailConfigModel? conf = null)
    {
        ResponseBaseModel res = new();
        conf ??= (await _parameter_storage.GetEmailConfig()).Conf;
        if (!conf!.IsConfigured)
        {
            res.AddError("Конфигурация не установлена");
            return res;
        }

        return await _email.ConnectSmtpAsync(conf);
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
#if DEBUG
           .WithTlsOptions(p => p.WithCertificateValidationHandler(sx => true))
#endif
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
        catch (MqttConnectingFailedException mcf)
        {
            res.AddError($"Failed to connect {mcf.Message}");
        }
        catch (Exception ex)
        {
            res.AddError($"Failed to connect {ex.Message}");
        }

        return res;
    }

    /// <inheritdoc/>
    public virtual async Task<TelegramBotCheckResponseModel> TestTelegramBotConnect(TelegramBotConfigModel? conf = null)
    {
        TelegramBotCheckResponseModel res = new();
        conf ??= (await _parameter_storage.GetTelegramBotConfig()).Conf;

        if (string.IsNullOrEmpty(conf?.TelegramBotToken))
        {
            res.AddError("TelegramBotToken не может быть пустым");
            return res;
        }

        TelegramBotClientOptions options = new(conf.TelegramBotToken);

        try
        {
            TelegramBotClient tbot_cli = new(options, _http_client);
            Telegram.Bot.Types.User _me = await tbot_cli.GetMeAsync();

            res.FirstName = _me.FirstName;
            res.LastName = _me.LastName;
            res.Username = _me.Username;
            res.Id = _me.Id;

            ITelegramBotClient? tbc = _service_provider.GetService<ITelegramBotClient>();

            if (tbc is not null)
            {
                Telegram.Bot.Types.User _demon_me = await tbc.GetMeAsync();
                res.ServiceIsRunning = new()
                {
                    FirstName = _demon_me.FirstName,
                    LastName = _demon_me.LastName,
                    Username = _demon_me.Username,
                    Id = _demon_me.Id
                };
            }
        }
        catch (Exception ex)
        {
            res.AddError(ex.Message);
        }

        return res;
    }

    /// <inheritdoc/>
    public async Task<MqttPublishMessageResultModel> PublishMqttMessage(MqttPublishMessageModel message) => await _mqttClientService.PublishMessage(message);
}