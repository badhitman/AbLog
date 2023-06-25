using Microsoft.Extensions.Logging;
using MQTTnet.Client;
using SharedLib;
using MQTTnet;

namespace ServerLib;

/// <summary>
/// 
/// </summary>
public class MqttClientService : MqttBaseService
{
    /// <inheritdoc/>
    public override MqttClientSubscribeOptions MqttSubscribeOptions => _mqttFactory
                .CreateSubscribeOptionsBuilder()
                .WithTopicFilter(f =>
                {
                    f.WithTopic(_mqtt_settings.Topic);
                    f.WithTopic(GlobalStatic.Commands.RESULT_CAMERAS);
                    f.WithTopic(GlobalStatic.Commands.RESULT_HTTP);
                    f.WithTopic(GlobalStatic.Commands.RESULT_SHOT);
                })
            .Build();

    /// <summary>
    /// 
    /// </summary>
    public MqttClientService(IMqttClient mqttClient, ILogger<MqttClientService> logger, MqttConfigModel mqtt_settings, MqttFactory mqttFactory)
        : base(mqttClient, logger, mqtt_settings, mqttFactory)
    {

    }
}
