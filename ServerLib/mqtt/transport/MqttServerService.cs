using Microsoft.Extensions.Logging;
using MQTTnet.Client;
using SharedLib;
using MQTTnet;

namespace ServerLib;

/// <summary>
/// 
/// </summary>
public class MqttServerService : MqttBaseService
{
    /// <inheritdoc/>
    public override MqttClientSubscribeOptions MqttSubscribeOptions => _mqttFactory
                .CreateSubscribeOptionsBuilder()
                .WithTopicFilter(f =>
                {
                    f.WithTopic(_mqtt_settings.Topic);
                    f.WithTopic(GlobalStatic.Commands.REQUEST_CAMERAS);
                    f.WithTopic(GlobalStatic.Commands.REQUEST_HTTP);
                    f.WithTopic(GlobalStatic.Commands.REQUEST_SHOT);
                })
            .Build();

    /// <summary>
    /// 
    /// </summary>
    public MqttServerService(IMqttClient mqttClient, ILogger<MqttServerService> logger, MqttConfigModel mqtt_settings, MqttFactory mqttFactory)
        : base(mqttClient, logger, mqtt_settings, mqttFactory)
    {

    }
}