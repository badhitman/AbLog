////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Tools: MQTT and Email
/// </summary>
public interface IToolsService
{
    /// <summary>
    /// Запустить MQTT службу
    /// </summary>
    public Task<ResponseBaseModel> StartMqtt(CancellationToken cancellationTokenMain);

    /// <summary>
    /// Остановить MQTT службу
    /// </summary>
    public Task<ResponseBaseModel> StopMqtt(CancellationToken cancellationTokenMain);

    /// <summary>
    /// Получить статус MQTT службы
    /// </summary>
    public Task<BoolResponseModel> StatusMqtt();

    /// <summary>
    /// Проверить подключение MQTT
    /// </summary>
    public Task<ResponseBaseModel> TestMqttConnect(MqttConfigModel? conf = null, CancellationToken cancellation_token = default);

    /// <summary>
    /// Тест Email подключения (SMTP)
    /// </summary>
    public Task<ResponseBaseModel> TestEmailConnect(EmailConfigModel? conf = null, CancellationToken cancellation_token = default);

    /// <summary>
    /// Тест TelegramBot токена
    /// </summary>
    public Task<TelegramBotCheckResponseModel> TestTelegramBotConnect(TelegramBotConfigModel? conf = null, CancellationToken cancellation_token = default);

    /// <summary>
    /// MQTT Message publish
    /// </summary>
    public Task<MqttPublishMessageResultModel> PublishMqttMessage(MqttPublishMessageModel message, CancellationToken cancellation_token);
}