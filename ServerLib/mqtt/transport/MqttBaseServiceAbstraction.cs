using Microsoft.Extensions.Logging;
using MQTTnet.Client;
using System.Text;
using SharedLib;
using MQTTnet;
using ab.context;
using Newtonsoft.Json;

namespace ServerLib;

/// <summary>
/// MQTT служба
/// </summary>
public abstract class MqttBaseServiceAbstraction : IMqttBaseService
{
    /// <summary>
    /// 
    /// </summary>
    protected ILogger<MqttBaseServiceAbstraction> _logger = default!;
    /// <summary>
    /// 
    /// </summary>
    protected readonly MqttConfigModel _mqtt_settings;
    /// <summary>
    /// 
    /// </summary>
    protected readonly MqttFactory _mqttFactory;
    /// <summary>
    /// 
    /// </summary>
    protected readonly IMqttClient _mqttClient;
    /// <summary>
    /// 
    /// </summary>
    protected MqttClientOptions mqttClientOptions;

    /// <summary>
    /// 
    /// </summary>
    public event Func<MqttApplicationMessageReceivedEventArgs, Task>? ApplicationMessageReceivedAsync;

    /// <inheritdoc/>
    public abstract MqttClientSubscribeOptions MqttSubscribeOptions { get; }

    /// <summary>
    /// MQTT служба
    /// </summary>
    public MqttBaseServiceAbstraction(IMqttClient mqttClient, MqttConfigModel mqtt_settings, MqttFactory mqttFactory)
    {
        _mqtt_settings = mqtt_settings;
        _mqttFactory = mqttFactory;
        _mqttClient = mqttClient;

        mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTls()
                .WithMaximumPacketSize(_mqtt_settings.MessageMaxSizeBytes)
                .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V500)
                .WithClientId(_mqtt_settings.ClientId)
                .WithTcpServer(_mqtt_settings.Server, _mqtt_settings.Port)
                .WithCredentials(_mqtt_settings.Username, _mqtt_settings.Password)
                .WithKeepAlivePeriod(TimeSpan.FromSeconds(10))
                .Build();
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> StartService()
    {
        ResponseBaseModel res = await StopService();
        try
        {
            // Create TCP based options using the builder.
            MqttClientOptions mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTls()
                .WithMaximumPacketSize(_mqtt_settings.MessageMaxSizeBytes)
                .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V500)
                .WithClientId(_mqtt_settings.ClientId)
                .WithTcpServer(_mqtt_settings.Server, _mqtt_settings.Port)
                .WithCredentials(_mqtt_settings.Username, _mqtt_settings.Password)
                .WithKeepAlivePeriod(TimeSpan.FromSeconds(10))
                .Build();

            _mqttClient.DisconnectedAsync += DisconnectedHandleAsync;
            _mqttClient.ApplicationMessageReceivedAsync += ApplicationMessageReceiveHandledAsync;

            MqttClientConnectResult ccr = await _mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

            if (ccr.ResultCode == MqttClientConnectResultCode.Success)
                res.AddSuccess($"Connect: {Enum.GetName(ccr.ResultCode)}; {ccr.ReasonString}".Trim());
            else
                res.AddError($"Connect: {Enum.GetName(ccr.ResultCode)}; {ccr.ReasonString}".Trim());

            MqttClientSubscribeResult csr = await _mqttClient.SubscribeAsync(MqttSubscribeOptions, CancellationToken.None);
#if DEBUG
            MqttApplicationMessage applicationMessage = new MqttApplicationMessageBuilder()
            .WithTopic(GlobalStatic.Commands.AB_LOG_SYSTEM)
            .WithUserProperty("test_prop_name", "test_prop_value")
            .WithPayload("19.5")
            .Build();

            MqttClientPublishResult cpr = await _mqttClient.PublishAsync(applicationMessage, CancellationToken.None);
#endif
        }
        catch (Exception ex)
        {
            _logger.LogError(nameof(StartService), ex);
            res.AddError(ex.Message);
        }
        return res;
    }

    /// <inheritdoc/>
    public virtual Task ApplicationMessageReceiveHandledAsync(MqttApplicationMessageReceivedEventArgs e)
    {
        _logger.LogInformation($"client:{e.ClientId}; topic:{e.ApplicationMessage.Topic};{e.ApplicationMessage.UserProperties}");
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
            ccr = await _mqttClient.ConnectAsync(_mqttClient.Options);
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> StopService()
    {
        ResponseBaseModel res = new();

        if (_mqttClient.IsConnected)
        {
            _mqttClient.DisconnectedAsync -= DisconnectedHandleAsync;
            _mqttClient.ApplicationMessageReceivedAsync -= ApplicationMessageReceiveHandledAsync;

            await _mqttClient.DisconnectAsync();
            res.AddSuccess("Соединение закрыто");
        }
        else
            res.AddInfo("Соединение отсутсвует (закрывать нечего)");

        using ParametersContext _context = new();
        string _mqttConfig = _context.GetStoredParameter(nameof(MqttConfigModel), "").StoredValue;
        MqttConfigModel mqtt_settings = JsonConvert.DeserializeObject<MqttConfigModel>(_mqttConfig) ?? new();

        if (_mqtt_settings != mqtt_settings)
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
    public BoolResponseModel StatusService()
    {
        BoolResponseModel res = new()
        {
            Response = _mqttClient.IsConnected
        };
        res.AddInfo(_mqttClient.IsConnected ? "Клиент подключён" : "Клиент не подключён");

        return res;
    }

    /// <inheritdoc/>
    public async Task<MqttPublishMessageResultModel> PublishMessage(MqttPublishMessageModel message)
    {
        MqttApplicationMessageBuilder msg = new MqttApplicationMessageBuilder()
            .WithRetainFlag(message.RetainFlag)
            .WithPayload(message.Payload);

            msg.WithTopic(GlobalStatic.Commands.AB_LOG_SYSTEM);

        if (message.CorrelationData?.Any() == true)
            msg.WithCorrelationData(message.CorrelationData);

        if (message.ResponseTopics?.Any() == true)
            foreach (string response_topic in message.ResponseTopics)
                msg.WithResponseTopic(response_topic);

        if (message.Topics.Any())
            foreach (string topic in message.Topics)
                msg.WithTopic(topic);

        if (message.UserProperties?.Any() == true)
            foreach (KeyValuePair<string, string> prop in message.UserProperties)
                msg.WithUserProperty(prop.Key, prop.Value);

        MqttApplicationMessage app_msg = msg.Build();
        MqttClientPublishResult cpr = await _mqttClient.PublishAsync(app_msg, CancellationToken.None);

        MqttPublishMessageResultModel res = new()
        {
            ReasonString = cpr.ReasonString,
            PacketIdentifier = cpr.PacketIdentifier,
            UserProperties = cpr.UserProperties.Select(x => new KeyValuePair<string, string>(x.Name, x.Value)).ToArray()
        };

        if (cpr.IsSuccess)
        {
            res.AddError("Не удалось отправить/опубликовать сообщение");
        }

        return res;
    }
}