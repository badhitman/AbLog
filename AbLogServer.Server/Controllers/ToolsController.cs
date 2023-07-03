using Microsoft.AspNetCore.Mvc;
using SharedLib;

namespace ABLog;

/// <summary>
/// Tools
/// </summary>
[ApiController]
[Route("/api/[controller]")]
public class ToolsController : ControllerBase
{
    readonly ILogger<ToolsController> _logger;
    readonly IToolsService _toolss_service;

    /// <summary>
    /// 
    /// </summary>
    public ToolsController(ILogger<ToolsController> logger, IToolsService tools_service)
    {
        _logger = logger;
        _toolss_service = tools_service;
    }

    /// <summary>
    /// Запустить MQTT службу
    /// </summary>
    [HttpGet($"{GlobalStatic.Routes.Mqtt}/{GlobalStatic.Routes.START}")]
    public async Task<ResponseBaseModel> StartMqtt() => await _toolss_service.StartMqtt();

    /// <summary>
    /// Остановить MQTT службу
    /// </summary>
    [HttpGet($"{GlobalStatic.Routes.Mqtt}/{GlobalStatic.Routes.STOP}")]
    public async Task<ResponseBaseModel> StopMqtt() => await _toolss_service.StopMqtt();

    /// <summary>
    /// Получить статус MQTT службы
    /// </summary>
    [HttpGet($"{GlobalStatic.Routes.Mqtt}/{GlobalStatic.Routes.STATUS}")]
    public async Task<BoolResponseModel> StatusMqtt() => await _toolss_service.StatusMqtt();

    /// <summary>
    /// Проверить подключение к Email (конфигурация imap+smtp)
    /// </summary>
    [HttpPost($"{GlobalStatic.Routes.Email}/{GlobalStatic.Routes.CHECK}")]
    public async Task<ResponseBaseModel> EmailConfigTestSmtpConnection(EmailConfigModel? email_conf) => await _toolss_service.TestEmailConnect(email_conf);

    /// <summary>
    /// Проверить подключение к Mqtt
    /// </summary>
    [HttpPost($"{GlobalStatic.Routes.Mqtt}/{GlobalStatic.Routes.CHECK}")]
    public async Task<ResponseBaseModel> MqttConfigTestConnection(MqttConfigModel? mqtt_conf) => await _toolss_service.TestMqttConnect(mqtt_conf);

    /// <summary>
    /// 
    /// </summary>
    [HttpPost($"{GlobalStatic.Routes.Mqtt}/{GlobalStatic.Routes.Mqtt}/{GlobalStatic.Routes.PUBLISH}")]
    public async Task<MqttPublishMessageResultModel> PublishMqttMessage(MqttPublishMessageModel message) => await _toolss_service.PublishMqttMessage(message);
}