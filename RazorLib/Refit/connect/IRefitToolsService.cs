using SharedLib;
using Refit;

namespace BlazorLib;

/// <summary>
/// Работа с бэком: Tools
/// </summary>
[Headers("Content-Type: application/json")]
public interface IRefitToolsService
{
    /// <summary>
    /// Запустить MQTT службу
    /// </summary>
    [Get($"/api/{GlobalStatic.HttpRoutes.Tools}/{GlobalStatic.HttpRoutes.Mqtt}/{GlobalStatic.HttpRoutes.START}")]
    public Task<ApiResponse<ResponseBaseModel>> StartMqtt();

    /// <summary>
    /// Остановить MQTT службу
    /// </summary>
    [Get($"/api/{GlobalStatic.HttpRoutes.Tools}/{GlobalStatic.HttpRoutes.Mqtt}/{GlobalStatic.HttpRoutes.STOP}")]
    public Task<ApiResponse<ResponseBaseModel>> StopMqtt();

    /// <summary>
    /// Получить статус MQTT службы
    /// </summary>
    [Get($"/api/{GlobalStatic.HttpRoutes.Tools}/{GlobalStatic.HttpRoutes.Mqtt}/{GlobalStatic.HttpRoutes.STATUS}")]
    public Task<ApiResponse<BoolResponseModel>> StatusMqtt();

    /// <summary>
    /// Проверить подключение к Email (конфигурация imap+smtp)
    /// </summary>
    [Post($"/api/{GlobalStatic.HttpRoutes.Tools}/{GlobalStatic.HttpRoutes.Email}/{GlobalStatic.HttpRoutes.CHECK}")]
    public Task<ApiResponse<ResponseBaseModel>> EmailConfigTestSmtpConnection(EmailConfigModel? email_conf);

    /// <summary>
    /// Проверить подключение к Mqtt
    /// </summary>
    [Post($"/api/{GlobalStatic.HttpRoutes.Tools}/{GlobalStatic.HttpRoutes.Mqtt}/{GlobalStatic.HttpRoutes.CHECK}")]
    public Task<ApiResponse<ResponseBaseModel>> MqttConfigTestSmtpConnection(MqttConfigModel? mqtt_conf);

    /// <summary>
    /// 
    /// </summary>
    [Post($"/api/{GlobalStatic.HttpRoutes.Tools}/{GlobalStatic.HttpRoutes.Mqtt}/{GlobalStatic.HttpRoutes.PUBLISH}")]
    public Task<ApiResponse<MqttPublishMessageResultModel>> PublishMqttMessage(MqttPublishMessageModel message);
}