using MQTTnet.Client;
using SharedLib;

namespace ServerLib;

/// <summary>
/// 
/// </summary>
public interface IMqttBaseService
{
    /// <summary>
    /// 
    /// </summary>
    public event Func<MqttApplicationMessageReceivedEventArgs, Task>? ApplicationMessageReceivedAsync;

    /// <summary>
    /// 
    /// </summary>
    public MqttClientSubscribeOptions MqttSubscribeOptions { get; }

    /// <summary>
    /// Запутить MQTT службу
    /// </summary>
    public Task<ResponseBaseModel> StartService();

    /// <summary>
    /// 
    /// </summary>
    public Task ApplicationMessageReceiveHandledAsync(MqttApplicationMessageReceivedEventArgs e);

    /// <summary>
    /// 
    /// </summary>
    public Task DisconnectedHandleAsync(MqttClientDisconnectedEventArgs e);

    /// <summary>
    /// Остановить MQTT службу
    /// </summary>
    public Task<ResponseBaseModel> StopService();

    /// <summary>
    /// Получить статус службы
    /// </summary>
    public BoolResponseModel StatusService();

    /// <summary>
    /// Опубликовать сообщение
    /// </summary>
    public Task<MqttPublishMessageResultModel> PublishMessage(MqttPublishMessageModel message);
}