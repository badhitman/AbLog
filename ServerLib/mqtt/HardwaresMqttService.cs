using System.Runtime.Versioning;
using Newtonsoft.Json;
using MQTTnet.Client;
using System.Text;
using SharedLib;
using MQTTnet;

namespace ServerLib;

/// <summary>
/// Устройства
/// </summary>
[SupportedOSPlatform("windows")]
[SupportedOSPlatform("linux")]
[SupportedOSPlatform("android")]
public class HardwaresMqttService : IHardwaresService
{
    readonly IMqttBaseService _mqtt;
    readonly MqttConfigModel _conf;
    readonly MqttFactory _mqttFactory;
    readonly IMqttClient _mqtt_client;

    /// <summary>
    /// Устройства IMqttClient
    /// </summary>
    public HardwaresMqttService(IMqttBaseService mqtt, MqttConfigModel conf, MqttFactory mqttFactory, IMqttClient mqtt_client)
    {
        _mqtt = mqtt;
        _conf = conf;
        _mqttFactory = mqttFactory;
        _mqtt_client = mqtt_client;
    }

    /// <inheritdoc/>
    public async Task<HardwaresResponseModel> HardwaresGetAll()
    {
        HardwaresResponseModel res = new();
        BoolResponseModel status = _mqtt.StatusService();
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
        MqttClientSubscribeResult subscr_res = await _mqtt_client.SubscribeAsync(subscr_opt, CancellationToken.None);
        MqttClientUnsubscribeOptions unsubscr_opt = _mqttFactory.CreateUnsubscribeOptionsBuilder()
            .WithTopicFilter(response_topic)
               .Build();
        MqttClientUnsubscribeResult unsubscr_res = await _mqtt_client.UnsubscribeAsync(unsubscr_opt);



        SimpleIdNoiseModel request = new();
        byte[] request_bytes = await CipherService.EncryptAsync(JsonConvert.SerializeObject(request), _conf.Secret ?? CipherService.DefaultSecret, msg_id);

        MqttPublishMessageModel p_msg = new(request_bytes, new[] { GlobalStatic.Commands.HARDWARES })
        {
            CorrelationData = Encoding.UTF8.GetBytes(msg_id),
            ResponseTopics = new string[] { response_topic }
        };

        Func<MqttApplicationMessageReceivedEventArgs, Task> MessageReceivedEvent = (MqttApplicationMessageReceivedEventArgs args)=> 
        { 
            if(args.ApplicationMessage.Topic.Equals(response_topic))
            {

            }


            return Task.CompletedTask; 
        };

        _mqtt.ApplicationMessageReceivedAsync += MessageReceivedEvent;

        MqttPublishMessageResultModel send_msg = await _mqtt.PublishMessage(p_msg);


        _mqtt.ApplicationMessageReceivedAsync -= MessageReceivedEvent;

        throw new NotImplementedException();
    }

    private Task ApplicationMessageReceivedHandlerAsync(MqttApplicationMessageReceivedEventArgs args)
    {
        

        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<EntriyResponseModel> CheckPortHardware(PortHardwareCheckRequestModel req)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<HttpResponseModel> GetHardwareHtmlPage(HardvareGetRequestModel req)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<ResponseBaseModel> HardwareDelete(int hardware_id)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<HardwareResponseModel> HardwareGet(int hardware_id)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<PortHardwareResponseModel> HardwarePortGet(int port_id)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<EntriesResponseModel> HardwaresGetAllAsEntries()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<EntriesNestedResponseModel> HardwaresGetTreeNestedEntries()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<HardwareResponseModel> HardwareUpdate(HardwareBaseModel hardware)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<ResponseBaseModel> SetNamePort(EntryModel port_id_name)
    {
        throw new NotImplementedException();
    }
}