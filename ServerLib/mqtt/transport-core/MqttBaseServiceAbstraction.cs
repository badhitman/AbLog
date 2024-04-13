////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using ab.context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Exceptions;
using Newtonsoft.Json;
using SharedLib;
using System.Diagnostics;
using System.Runtime.Versioning;
using System.Text;

namespace ServerLib;

/// <summary>
/// MQTT служба
/// </summary>
[SupportedOSPlatform("windows")]
[SupportedOSPlatform("linux")]
[SupportedOSPlatform("android")]
public abstract class MqttBaseServiceAbstraction(IMqttClient mqttClient, MqttConfigModel mqtt_settings, MqttFactory mqttFactory, ILogger<MqttBaseServiceAbstraction> logger, INotifyService notifyService, IDbContextFactory<ParametersContext> dbFactory) : IMqttBaseService
{
    /// <inheritdoc/>
    protected IDbContextFactory<ParametersContext> _dbFactory = dbFactory;

    /// <inheritdoc/>
    protected ILogger<MqttBaseServiceAbstraction> _logger = logger;
    /// <inheritdoc/>
    protected readonly MqttConfigModel _mqtt_settings = mqtt_settings;
    /// <inheritdoc/>
    protected readonly MqttFactory _mqttFactory = mqttFactory;
    /// <inheritdoc/>
    protected readonly IMqttClient _mqttClient = mqttClient;

    /// <inheritdoc/>
    public event Func<MqttApplicationMessageReceivedEventArgs, Task>? ApplicationMessageReceivedAsync;

    /// <inheritdoc/>
    public abstract MqttClientSubscribeOptions MqttSubscribeOptions { get; }
    /// <inheritdoc/>
    protected MqttClientOptions MqttClientOptions => new MqttClientOptionsBuilder()
#if DEBUG
                .WithTlsOptions(p => p.WithCertificateValidationHandler(sx => true))
#endif
                .WithMaximumPacketSize(_mqtt_settings.MessageMaxSizeBytes)
                .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V500)
                .WithClientId(_mqtt_settings.ClientId)
                .WithTcpServer(_mqtt_settings.Server, _mqtt_settings.Port)
                .WithCredentials(_mqtt_settings.Username, _mqtt_settings.Password)
                .WithKeepAlivePeriod(TimeSpan.FromSeconds(10))
                .Build();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> StartService(CancellationToken cancellation_token)
    {
        ResponseBaseModel res = await StopService(cancellation_token);
        _logger.LogInformation($"call >> {nameof(StartService)}");
        try
        {
            _mqttClient.DisconnectedAsync += DisconnectedHandleAsync;
            _mqttClient.ApplicationMessageReceivedAsync += ApplicationMessageReceiveHandledAsync;

            MqttClientConnectResult ccr;
            string msg;
            try
            {
                ccr = await _mqttClient.ConnectAsync(MqttClientOptions, cancellation_token);
            }
            catch (Exception ex)
            {
                msg = $"{MqttClientOptions.Credentials.GetUserName(MqttClientOptions)}:{MqttClientOptions.Credentials.GetPassword(MqttClientOptions)}";
                _logger.LogError(ex, msg);
                res.AddError(msg);
                return res;
            }



            if (ccr.ResultCode == MqttClientConnectResultCode.Success)
                res.AddSuccess($"Connect: {Enum.GetName(ccr.ResultCode)}; {ccr.ReasonString}".Trim());
            else
                res.AddError($"Connect: {Enum.GetName(ccr.ResultCode)}; {ccr.ReasonString}".Trim());

            MqttClientSubscribeResult csr = await _mqttClient.SubscribeAsync(MqttSubscribeOptions, cancellation_token);
            _logger.LogWarning($"{csr.ReasonString} _mqttClient.Subscribe >> {JsonConvert.SerializeObject(MqttSubscribeOptions.TopicFilters.Select(x => x.Topic))}");
        }
        catch (MqttCommunicationException ex)
        {
            _logger.LogError(ex, nameof(StartService));
            res.AddError(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(StartService));
            res.AddError(ex.Message);
        }
        return res;
    }

    /// <inheritdoc/>
    public virtual Task ApplicationMessageReceiveHandledAsync(MqttApplicationMessageReceivedEventArgs e)
    {
        _logger.LogInformation($"client:{e.ClientId}; topic:{e.ApplicationMessage.Topic};");
        if (e.ApplicationMessage.PayloadSegment.Array is not null)
            _logger.LogInformation(Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment.Array));
        if (ApplicationMessageReceivedAsync is not null)
            return ApplicationMessageReceivedAsync(e);
        else
            return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public async Task DisconnectedHandleAsync(MqttClientDisconnectedEventArgs e)
    {
        _logger.LogWarning($"mqttClient.DisconnectedAsync => ClientWasConnected:{e.ClientWasConnected}");
        MqttClientConnectResult ccr;
        if (e.ClientWasConnected)
        {
            ccr = await _mqttClient.ConnectAsync(_mqttClient.Options);
            if (ccr.ResultCode == MqttClientConnectResultCode.Success)
                _logger.LogInformation($"Connect: {Enum.GetName(ccr.ResultCode)}; {ccr.ReasonString}".Trim());
            else
                _logger.LogError($"Connect: {Enum.GetName(ccr.ResultCode)}; {ccr.ReasonString}".Trim());
        }
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> StopService(CancellationToken cancellation_token)
    {
        ResponseBaseModel res = new();
        _logger.LogInformation($"call >> {nameof(StopService)}");
        if (_mqttClient.IsConnected)
        {
            _mqttClient.DisconnectedAsync -= DisconnectedHandleAsync;
            _mqttClient.ApplicationMessageReceivedAsync -= ApplicationMessageReceiveHandledAsync;

            await _mqttClient.DisconnectAsync(cancellationToken: cancellation_token);
            res.AddSuccess("Соединение закрыто");
        }
        else
            res.AddInfo("Соединение отсутсвует (закрывать нечего)");

        using ParametersContext _context = await _dbFactory.CreateDbContextAsync(cancellation_token);
        string _mqttConfig = _context.GetStoredParameter(nameof(MqttConfigModel), "").StoredValue;
        MqttConfigModel? mqtt_settings = JsonConvert.DeserializeObject<MqttConfigModel>(_mqttConfig);

        if (mqtt_settings is not null && _mqtt_settings != mqtt_settings)
        {
            _mqtt_settings.Username = mqtt_settings.Username;
            _mqtt_settings.Password = mqtt_settings.Password;
            _mqtt_settings.Server = mqtt_settings.Server;
            _mqtt_settings.Port = mqtt_settings.Port;
            _mqtt_settings.ClientId = mqtt_settings.ClientId;
            _mqtt_settings.Secret = mqtt_settings.Secret;
            _mqtt_settings.MessageMaxSizeBytes = mqtt_settings.MessageMaxSizeBytes;
            _mqtt_settings.AutoStart = mqtt_settings.AutoStart;

            res.AddWarning("Конфигурация подключения изменилась!");
        }

        return res;
    }

    /// <inheritdoc/>
    public Task<BoolResponseModel> StatusService()
    {
        BoolResponseModel res = new()
        {
            Response = _mqttClient.IsConnected
        };

        if (_mqttClient.IsConnected)
            res.AddInfo("Клиент подключён");
        else
            res.AddError("Клиент не подключён");

        return Task.FromResult(res);
    }

    /// <inheritdoc/>
    public async Task<MqttPublishMessageResultModel> PublishMessage(MqttPublishMessageModel message, CancellationToken cancellation_token)
    {
        _logger.LogInformation($"call >> {nameof(PublishMessage)}");
        MqttApplicationMessageBuilder msg = new MqttApplicationMessageBuilder()
            .WithRetainFlag(message.RetainFlag)
            .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce)
            .WithPayload(message.Payload)
            .WithTopic(GlobalStatic.Routes.AB_LOG_SYSTEM);

        if (message.CorrelationData?.Any() == true)
            msg.WithCorrelationData(message.CorrelationData);

        if (message.ResponseTopics?.Any() == true)
            foreach (string response_topic in message.ResponseTopics)
                msg.WithResponseTopic(response_topic);

        if (message.Topics.Length != 0)
            foreach (string topic in message.Topics)
                msg.WithTopic(topic);

        if (message.UserProperties?.Any() == true)
            foreach (KeyValuePair<string, string> prop in message.UserProperties)
                msg.WithUserProperty(prop.Key, prop.Value);

        MqttApplicationMessage app_msg = msg.Build();
        MqttClientPublishResult cpr = await _mqttClient.PublishAsync(app_msg, cancellation_token);

        MqttPublishMessageResultModel res = new()
        {
            ReasonString = cpr.ReasonString,
            PacketIdentifier = cpr.PacketIdentifier,
            UserProperties = cpr.UserProperties.Select(x => new KeyValuePair<string, string>(x.Name, x.Value)).ToArray()
        };

        if (!cpr.IsSuccess)
            res.AddError("Не удалось отправить/опубликовать сообщение");

        return res;
    }

    /// <inheritdoc/>
    public async Task<SimpleStringResponseModel> MqttRemoteCall(object request, string topic, CancellationToken cancellation_token)
    {
        SimpleStringResponseModel res = new();
        BoolResponseModel status = await StatusService();
        if (!status.Response)
        {
            res.AddMessages(status.Messages);
            return res;
        }

        string msg_id = Guid.NewGuid().ToString();
        string response_topic = Guid.NewGuid().ToString();

        MqttClientSubscribeOptions subscr_opt = _mqttFactory.CreateSubscribeOptionsBuilder()
            .WithTopicFilter(f => { f.WithTopic(response_topic); })
            .Build();

        MqttClientSubscribeResult? subscr_res = await _mqttClient.SubscribeAsync(subscr_opt, cancellation_token);
        byte[] request_bytes = await CipherService.EncryptAsync(JsonConvert.SerializeObject(request), _mqtt_settings.Secret ?? CipherService.DefaultSecret, msg_id);

        MqttPublishMessageModel p_msg = new(request_bytes, new[] { topic })
        {
            CorrelationData = Encoding.UTF8.GetBytes(msg_id),
            ResponseTopics = [response_topic]
        };

        async Task MessageReceivedEvent(MqttApplicationMessageReceivedEventArgs e)
        {
            if (e.ApplicationMessage.Topic.Equals(response_topic))
            {
                byte[] payload_bytes = await CipherService.DecryptAsync([.. e.ApplicationMessage.PayloadSegment], _mqtt_settings.Secret ?? CipherService.DefaultSecret, e.ApplicationMessage.CorrelationData);
                res.TextPayload = Encoding.UTF8.GetString(payload_bytes);
            }
        }

        _mqttClient.ApplicationMessageReceivedAsync += MessageReceivedEvent;
        MqttPublishMessageResultModel send_msg;
        try
        {
            send_msg = await PublishMessage(p_msg, cancellation_token);
        }
        catch (Exception ex)
        {
            res.AddError(ex.Message);
            return res;
        }
        Stopwatch stopwatch = new();
        stopwatch.Start();
        int mqtt_dbg_step = 0;
        while (res.TextPayload is null)
        {
            await Task.Delay(100, cancellation_token);

            if (++mqtt_dbg_step > 15)
            {
                mqtt_dbg_step = 0;
                notifyService.MqttDebug((stopwatch.Elapsed, null, topic));
            }


        }
        notifyService.MqttDebug((stopwatch.Elapsed, res.TextPayload, topic));
        stopwatch.Stop();

        _mqttClient.ApplicationMessageReceivedAsync -= MessageReceivedEvent;
        MqttClientUnsubscribeOptions unsubscr_opt = _mqttFactory.CreateUnsubscribeOptionsBuilder()
            .WithTopicFilter(response_topic)
               .Build();
        MqttClientUnsubscribeResult unsubscr_res = await _mqttClient.UnsubscribeAsync(unsubscr_opt, cancellation_token);

        return res;
    }
}