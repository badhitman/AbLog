////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Mvc;
using SharedLib;

namespace ABLogWeb;

/// <summary>
/// Tools
/// </summary>
[ApiController]
[Route("/api/[controller]")]
public class ToolsController(IToolsService tools_service) : ControllerBase
{
    /// <summary>
    /// Запустить MQTT службу
    /// </summary>
    [HttpGet($"/{GlobalStatic.Routes.Tools}/{GlobalStatic.Routes.Mqtt}/{GlobalStatic.Routes.START}")]
    public async Task<ResponseBaseModel> StartMqtt(CancellationToken cancellationToken = default)
        => await tools_service.StartMqtt(cancellationToken);

    /// <summary>
    /// Остановить MQTT службу
    /// </summary>
    [HttpGet($"/{GlobalStatic.Routes.Tools}/{GlobalStatic.Routes.Mqtt}/{GlobalStatic.Routes.STOP}")]
    public async Task<ResponseBaseModel> StopMqtt(CancellationToken cancellationToken = default)
        => await tools_service.StopMqtt(cancellationToken);

    /// <summary>
    /// Получить статус MQTT службы
    /// </summary>
    [HttpGet($"/{GlobalStatic.Routes.Tools}/{GlobalStatic.Routes.Mqtt}/{GlobalStatic.Routes.STATUS}")]
    public async Task<BoolResponseModel> StatusMqtt()
        => await tools_service.StatusMqtt();

    /// <summary>
    /// Проверить подключение к Email (конфигурация smtp)
    /// </summary>
    [HttpPost($"/{GlobalStatic.Routes.Tools}/{GlobalStatic.Routes.Email}/{GlobalStatic.Routes.CHECK}-{GlobalStatic.Routes.CONFIG}")]
    public async Task<ResponseBaseModel> EmailConfigTestSmtpConnection(EmailConfigModel? email_conf, CancellationToken cancellation_token = default)
        => await tools_service.TestEmailConnect(email_conf, cancellation_token);

    /// <summary>
    /// Проверить подключение к Mqtt
    /// </summary>
    [HttpPost($"/{GlobalStatic.Routes.Tools}/{GlobalStatic.Routes.Mqtt}/{GlobalStatic.Routes.CHECK}-{GlobalStatic.Routes.CONFIG}")]
    public async Task<ResponseBaseModel> MqttConfigTestConnection(MqttConfigModel? mqtt_conf, CancellationToken cancellation_token = default)
        => await tools_service.TestMqttConnect(mqtt_conf, cancellation_token);

    /// <summary>
    /// 
    /// </summary>
    [HttpPost($"/{GlobalStatic.Routes.Tools}/{GlobalStatic.Routes.Mqtt}/{GlobalStatic.Routes.MESSAGE}-{GlobalStatic.Routes.PUBLISH}")]
    public async Task<MqttPublishMessageResultModel> PublishMqttMessage(MqttPublishMessageModel message, CancellationToken cancellation_token = default)
        => await tools_service.PublishMqttMessage(message, cancellation_token);

    /// <summary>
    /// Проверить токен TelegramBot
    /// </summary>
    [HttpPost($"/{GlobalStatic.Routes.Tools}/{GlobalStatic.Routes.TelegramBot}/{GlobalStatic.Routes.CHECK}-{GlobalStatic.Routes.CONFIG}")]
    public async Task<TelegramBotCheckResponseModel> TelegramBotConfigTestConnection(TelegramBotConfigModel? telegram_bot_conf, CancellationToken cancellation_token = default)
        => await tools_service.TestTelegramBotConnect(telegram_bot_conf, cancellation_token);
}