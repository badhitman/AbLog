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
    [Get($"/api/{GlobalStatic.Routes.Tools}/{GlobalStatic.Routes.Mqtt}/{GlobalStatic.Routes.START}")]
    public Task<ApiResponse<ResponseBaseModel>> StartMqtt();

    /// <summary>
    /// Остановить MQTT службу
    /// </summary>
    [Get($"/api/{GlobalStatic.Routes.Tools}/{GlobalStatic.Routes.Mqtt}/{GlobalStatic.Routes.STOP}")]
    public Task<ApiResponse<ResponseBaseModel>> StopMqtt();

    /// <summary>
    /// Получить статус MQTT службы
    /// </summary>
    [Get($"/api/{GlobalStatic.Routes.Tools}/{GlobalStatic.Routes.Mqtt}/{GlobalStatic.Routes.STATUS}")]
    public Task<ApiResponse<BoolResponseModel>> StatusMqtt();

    /// <summary>
    /// Проверить подключение к Email (конфигурация imap+smtp)
    /// </summary>
    [Post($"/api/{GlobalStatic.Routes.Tools}/{GlobalStatic.Routes.Email}/{GlobalStatic.Routes.CHECK}")]
    public Task<ApiResponse<ResponseBaseModel>> EmailConfigTestSmtpConnection(EmailConfigModel? email_conf);

    /// <summary>
    /// Проверить подключение к Mqtt
    /// </summary>
    [Post($"/api/{GlobalStatic.Routes.Tools}/{GlobalStatic.Routes.Mqtt}/{GlobalStatic.Routes.CHECK}")]
    public Task<ApiResponse<ResponseBaseModel>> MqttConfigTestSmtpConnection(MqttConfigModel? mqtt_conf);

    /// <summary>
    /// 
    /// </summary>
    [Post($"/api/{GlobalStatic.Routes.Tools}/{GlobalStatic.Routes.Mqtt}/{GlobalStatic.Routes.PUBLISH}")]
    public Task<ApiResponse<MqttPublishMessageResultModel>> PublishMqttMessage(MqttPublishMessageModel message);
}