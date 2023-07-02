using Microsoft.Extensions.Logging;
using MQTTnet.Client;
using SharedLib;
using MQTTnet;
using System.Runtime.Versioning;
using System.Text;
using Newtonsoft.Json;

namespace ServerLib;

/// <summary>
/// 
/// </summary>
[SupportedOSPlatform("windows")]
[SupportedOSPlatform("linux")]
[SupportedOSPlatform("android")]
public class MqttServerService : MqttBaseServiceAbstraction
{
    readonly IHardwaresService _hardwares_service;

    /// <inheritdoc/>
    public override MqttClientSubscribeOptions MqttSubscribeOptions => _mqttFactory
                .CreateSubscribeOptionsBuilder()
                .WithTopicFilter(f =>
                {
                    f.WithTopic(GlobalStatic.Commands.AB_LOG_SYSTEM);
                    f.WithTopic(GlobalStatic.Commands.CAMERAS);
                    f.WithTopic(GlobalStatic.Commands.HTTP);
                    f.WithTopic(GlobalStatic.Commands.SHOT);
                    f.WithTopic(GlobalStatic.Commands.HARDWARES);
                })
            .Build();

    /// <summary>
    /// 
    /// </summary>
    public MqttServerService(IMqttClient mqttClient, ILogger<MqttServerService> logger, MqttConfigModel mqtt_settings, MqttFactory mqttFactory, IHardwaresService hardwares_service, CancellationToken cancellation_token = default)
        : base(mqttClient, mqtt_settings, mqttFactory, logger, cancellation_token)
    {
        _logger = logger;
        _hardwares_service = hardwares_service;
    }

    /// <summary>
    /// 
    /// </summary>
    public override async Task ApplicationMessageReceiveHandledAsync(MqttApplicationMessageReceivedEventArgs e)
    {
        await base.ApplicationMessageReceiveHandledAsync(e);
        _logger.LogInformation($"call >> {nameof(ApplicationMessageReceiveHandledAsync)}");
        byte[] payload_bytes = e.ApplicationMessage.PayloadSegment.ToArray();

        try
        {
            payload_bytes = await CipherService.DecryptAsync(payload_bytes, this._mqtt_settings.Secret ?? CipherService.DefaultSecret, e.ApplicationMessage.CorrelationData);
        }
        catch (Exception ex)
        {
            _logger.LogError("", ex);
            return;
        }


        string payload_json = Encoding.UTF8.GetString(payload_bytes);
        string salt = Guid.NewGuid().ToString();
        switch (e.ApplicationMessage.Topic)
        {
            case GlobalStatic.Commands.HARDWARES:
                SimpleIdNoiseModel? req = JsonConvert.DeserializeObject<SimpleIdNoiseModel>(payload_json);

                if (req is null)
                {
                    _logger.LogError("req is null. error 1C55124E-8DF4-4CCC-A9FF-8CC2C0AF6B65");
                    return;
                }

                string response_json;
                if (req.Id <= 0)
                {
                    HardwaresResponseModel hardwares = await _hardwares_service.HardwaresGetAll();
                    response_json = JsonConvert.SerializeObject(hardwares);
                }
                else
                {
                    HardwareResponseModel hw = await _hardwares_service.HardwareGet(req.Id);
                    response_json = JsonConvert.SerializeObject(hw);
                }

                payload_bytes = await CipherService.EncryptAsync(response_json, this._mqtt_settings.Secret ?? CipherService.DefaultSecret, salt);
                MqttPublishMessageModel pub_msg = new(payload_bytes, new[] { e.ApplicationMessage.ResponseTopic })
                {
                    CorrelationData = Encoding.UTF8.GetBytes(salt),
                };

                MqttPublishMessageResultModel pub_red = await PublishMessage(pub_msg);

                if (!pub_red.IsSuccess)
                    _logger.LogError($"!pub_red.IsSuccess ({pub_red.Message}). error {{07CFEFD6-1F99-4082-925B-7F636BB6CC0A}}");

                break;
            case GlobalStatic.Commands.HTTP:

                break;
            case GlobalStatic.Commands.SHOT:

                break;
            case GlobalStatic.Commands.CAMERAS:

                break;
            case GlobalStatic.Commands.AB_LOG_SYSTEM:

                break;
        }
    }
}