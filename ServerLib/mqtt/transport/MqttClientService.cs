using Microsoft.Extensions.Logging;
using System.Runtime.Versioning;
using MQTTnet.Client;
using SharedLib;
using MQTTnet;

namespace ServerLib;

/// <summary>
/// 
/// </summary>
[SupportedOSPlatform("windows")]
[SupportedOSPlatform("linux")]
[SupportedOSPlatform("android")]
public class MqttClientService : MqttBaseServiceAbstraction
{
    /// <inheritdoc/>
    public override MqttClientSubscribeOptions MqttSubscribeOptions => _mqttFactory
                .CreateSubscribeOptionsBuilder()        
                .WithTopicFilter(f =>
                {
                    f.WithTopic(GlobalStatic.Commands.AB_LOG_SYSTEM);
                })
            .Build();

    /// <summary>
    /// 
    /// </summary>
    public MqttClientService(IMqttClient mqttClient, ILogger<MqttClientService> logger, MqttConfigModel mqtt_settings, MqttFactory mqttFactory, CancellationToken cancellation_token = default)
        : base(mqttClient, mqtt_settings, mqttFactory, logger, cancellation_token)
    {
        _logger = logger;
    }

    /// <summary>
    /// 
    /// </summary>
    public override Task ApplicationMessageReceiveHandledAsync(MqttApplicationMessageReceivedEventArgs e)
    {


        return base.ApplicationMessageReceiveHandledAsync(e);
    }
}
