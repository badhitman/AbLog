using Microsoft.Extensions.Logging;
using System.Runtime.Versioning;
using Newtonsoft.Json;
using MQTTnet.Client;
using System.Text;
using SharedLib;
using MQTTnet;
using MQTTnet.Packets;

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
                .WithTopicFilter(f => { f.WithTopic(GlobalStatic.Routes.AB_LOG_SYSTEM); })
                .WithTopicFilter(f => { f.WithTopic(GlobalStatic.Routes.Cameras); })
                .WithTopicFilter(f => { f.WithTopic(GlobalStatic.Routes.HTTP); })
                .WithTopicFilter(f => { f.WithTopic(GlobalStatic.Routes.SHOT); })
                .WithTopicFilter(f => { f.WithTopic($"{GlobalStatic.Routes.Hardwares}/{GlobalStatic.Routes.LIST}"); })
                .WithTopicFilter(f => { f.WithTopic($"{GlobalStatic.Routes.Hardware}/{GlobalStatic.Routes.GET}"); })
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
        byte[] payload_bytes = await CipherService.DecryptAsync(e.ApplicationMessage.PayloadSegment.ToArray(), _mqtt_settings.Secret ?? CipherService.DefaultSecret, e.ApplicationMessage.CorrelationData);

        string payload_json = Encoding.UTF8.GetString(payload_bytes);
        string salt = Guid.NewGuid().ToString();

        switch (e.ApplicationMessage.Topic)
        {
            case $"{GlobalStatic.Routes.Hardwares}/{GlobalStatic.Routes.LIST}":
                NoiseModel? req = JsonConvert.DeserializeObject<NoiseModel>(payload_json);

                if (req is null)
                {
                    _logger.LogError("req is null. error 1C55124E-8DF4-4CCC-A9FF-8CC2C0AF6B65");
                    return;
                }

                await PublishMessage(JsonConvert.SerializeObject(await _hardwares_service.HardwaresGetAll()), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);

                break;
            case $"{GlobalStatic.Routes.Hardware}/{GlobalStatic.Routes.GET}":
                SimpleIdNoiseModel? req_nid = JsonConvert.DeserializeObject<SimpleIdNoiseModel>(payload_json);

                if (req_nid is null)
                {
                    _logger.LogError("req is null. error 98C12EAC-73A9-4946-8850-7B075613833E");
                    return;
                }

                await PublishMessage(JsonConvert.SerializeObject(await _hardwares_service.HardwareGet(req_nid.Id)), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);

                break;
            case GlobalStatic.Routes.HTTP:

                break;
            case GlobalStatic.Routes.SHOT:

                break;
            case GlobalStatic.Routes.Cameras:

                break;
            case GlobalStatic.Routes.AB_LOG_SYSTEM:

                break;
        }
    }

    /// <inheritdoc/>
    public async Task PublishMessage(string body_string, string topic, string? secret, string salt)
    {
        byte[] payload_bytes = await CipherService.EncryptAsync(body_string, secret ?? CipherService.DefaultSecret, salt);
        MqttPublishMessageModel pub_msg = new(payload_bytes, new[] { topic })
        {
            CorrelationData = Encoding.UTF8.GetBytes(salt),
        };

        MqttPublishMessageResultModel pub_res = await PublishMessage(pub_msg);

        if (!pub_res.IsSuccess)
            _logger.LogError($"!pub_red.IsSuccess ({pub_res.Message}). error FAB191D8-4648-4705-AEFF-DF0AB762E527");
    }
}
/*
EntriesResponseModel HardwaresGetAllAsEntries();
EntriesNestedResponseModel HardwaresGetTreeNestedEntries();

ResponseBaseModel HardwareDelete(int hardware_id);
HardwareResponseModel HardwareUpdate(HardwareBaseModel hardware);
PortHardwareResponseModel HardwarePortGet(int port_id);
HttpResponseModel GetHardwareHtmlPage(HardvareGetRequestModel req);
EntriyResponseModel CheckPortHardware(PortHardwareCheckRequestModel req);
ResponseBaseModel SetNamePort(EntryModel port_id_name);
 */