using Microsoft.Extensions.Logging;
using MQTTnet.Client;
using SharedLib;
using MQTTnet;

namespace ServerLib;

/// <summary>
/// 
/// </summary>
public class MqttClientService : MqttBaseServiceAbstraction
{
    /// <inheritdoc/>
    public override MqttClientSubscribeOptions MqttSubscribeOptions => _mqttFactory
                .CreateSubscribeOptionsBuilder()
                .WithTopicFilter(f =>
                {
                    f.WithTopic(GlobalStatic.Commands.AB_LOG_SYSTEM);
                    //f.WithTopic(GlobalStatic.Commands.RESULT_CAMERAS);
                    //f.WithTopic(GlobalStatic.Commands.RESULT_HTTP);
                    //f.WithTopic(GlobalStatic.Commands.RESULT_SHOT);
                })
            .Build();

    /// <summary>
    /// 
    /// </summary>
    public MqttClientService(IMqttClient mqttClient, ILogger<MqttClientService> logger, MqttConfigModel mqtt_settings, MqttFactory mqttFactory)
        : base(mqttClient, mqtt_settings, mqttFactory)
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
