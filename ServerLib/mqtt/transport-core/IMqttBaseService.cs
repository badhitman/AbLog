////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using MQTTnet.Client;
using SharedLib;

namespace ServerLib;

/// <summary>
/// Mqtt base service
/// </summary>
public interface IMqttBaseService
{
    /// <inheritdoc/>
    public event Func<MqttApplicationMessageReceivedEventArgs, Task>? ApplicationMessageReceivedAsync;

    /// <summary>
    /// MQTT subscribe options
    /// </summary>
    public MqttClientSubscribeOptions MqttSubscribeOptions { get; }

    /// <summary>
    /// Запутить MQTT службу
    /// </summary>
    public Task<ResponseBaseModel> StartService(CancellationToken cancellation_token);

    /// <summary>
    /// Application message receive handled
    /// </summary>
    public Task ApplicationMessageReceiveHandledAsync(MqttApplicationMessageReceivedEventArgs e);

    /// <summary>
    /// Disconnected handle
    /// </summary>
    public Task DisconnectedHandleAsync(MqttClientDisconnectedEventArgs e);

    /// <summary>
    /// Остановить MQTT службу
    /// </summary>
    public Task<ResponseBaseModel> StopService(CancellationToken cancellation_token);

    /// <summary>
    /// Получить статус службы
    /// </summary>
    public Task<BoolResponseModel> StatusService();

    /// <summary>
    /// Опубликовать сообщение
    /// </summary>
    public Task<MqttPublishMessageResultModel> PublishMessage(MqttPublishMessageModel message, CancellationToken cancellation_token);

    /// <summary>
    /// Удалённый вызов серверного функционала через MQTT
    /// </summary>
    public Task<SimpleStringResponseModel> MqttRemoteCall(object request, string topic, CancellationToken cancellation_token);
}