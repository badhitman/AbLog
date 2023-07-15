////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using MQTTnet.Client;
using SharedLib;
using MQTTnet;
using Telegram.Bot;
using MQTTnet.Adapter;

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

    /// <summary>
    /// 
    /// </summary>
    public ToolsLocalService(IMqttBaseService mqttClientService, IParametersStorageService parameter_storage, MqttFactory mqtt_fact, HttpClient http_client)
    {
        _mqttClientService = mqttClientService;
        _parameter_storage = parameter_storage;
        _mqtt_fact = mqtt_fact;
        _http_client = http_client;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> StartMqtt() => await _mqttClientService.StartService();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> StopMqtt() => await _mqttClientService.StopService();

    /// <inheritdoc/>
    public Task<BoolResponseModel> StatusMqtt() => _mqttClientService.StatusService();

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
        MqttClientOptionsBuilderTlsParameters obtp = new()
        {
            AllowUntrustedCertificates = true,
            IgnoreCertificateChainErrors = true,
            IgnoreCertificateRevocationErrors = true
        };
        MqttClientOptions mqttClientOptions = new MqttClientOptionsBuilder()
           .WithTls(obtp)
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
        catch(MqttConnectingFailedException mcf)
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
    public virtual async Task<DictionaryResponseModel> TestTelegramBotConnect(TelegramBotConfigModel? conf = null)
    {
        DictionaryResponseModel res = new();
        conf ??= (await _parameter_storage.GetTelegramBotConfig()).Conf;

        if (string.IsNullOrEmpty(conf?.TelegramBotToken))
        {
            res.AddError("TelegramBotToken не может быть пустым");
            return res;
        }

        TelegramBotClientOptions options = new(conf.TelegramBotToken);

        if (!conf!.IsConfigured)
        {
            res.AddError("Конфигурация не установлена");
            return res;
        }

        try
        {
            TelegramBotClient tbot_cli = new(options, _http_client);
            Telegram.Bot.Types.User _me = await tbot_cli.GetMeAsync();
            res.DictionaryResponse = new Dictionary<string, object?>
            {
                { nameof(_me.FirstName), _me.FirstName },
                { nameof(_me.Username), _me.Username },
                { nameof(_me.Id), _me.Id },
                { nameof(_me.IsBot), _me.IsBot }
            };
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