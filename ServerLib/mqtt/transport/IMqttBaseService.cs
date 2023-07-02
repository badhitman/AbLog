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
    public Task<ResponseBaseModel> StartService(CancellationToken cancellation_token = default);

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
    public Task<ResponseBaseModel> StopService(CancellationToken cancellation_token = default);

    /// <summary>
    /// Получить статус службы
    /// </summary>
    public BoolResponseModel StatusService();

    /// <summary>
    /// Опубликовать сообщение
    /// </summary>
    public Task<MqttPublishMessageResultModel> PublishMessage(MqttPublishMessageModel message, CancellationToken cancellation_token = default);

    /// <summary>
    /// Удалённый вызов серверного функционала через MQTT
    /// </summary>
    public Task<SimpleStringResponseModel> MqttRemoteCall(object request, string topic, CancellationToken cancellation_token = default);
}