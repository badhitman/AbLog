////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

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
    readonly IToolsService _tools_service;

    /// <summary>
    /// 
    /// </summary>
    public ToolsController(ILogger<ToolsController> logger, IToolsService tools_service)
    {
        _logger = logger;
        _tools_service = tools_service;
    }

    /// <summary>
    /// Запустить MQTT службу
    /// </summary>
    [HttpGet($"{GlobalStatic.Routes.Mqtt}/{GlobalStatic.Routes.START}")]
    public async Task<ResponseBaseModel> StartMqtt() => await _tools_service.StartMqtt();

    /// <summary>
    /// Остановить MQTT службу
    /// </summary>
    [HttpGet($"{GlobalStatic.Routes.Mqtt}/{GlobalStatic.Routes.STOP}")]
    public async Task<ResponseBaseModel> StopMqtt() => await _tools_service.StopMqtt();

    /// <summary>
    /// Получить статус MQTT службы
    /// </summary>
    [HttpGet($"{GlobalStatic.Routes.Mqtt}/{GlobalStatic.Routes.STATUS}")]
    public async Task<BoolResponseModel> StatusMqtt() => await _tools_service.StatusMqtt();

    /// <summary>
    /// Проверить подключение к Email (конфигурация smtp)
    /// </summary>
    [HttpPost($"{GlobalStatic.Routes.Email}/{GlobalStatic.Routes.CHECK}")]
    public async Task<ResponseBaseModel> EmailConfigTestSmtpConnection(EmailConfigModel? email_conf) => await _tools_service.TestEmailConnect(email_conf);

    /// <summary>
    /// Проверить подключение к Mqtt
    /// </summary>
    [HttpPost($"{GlobalStatic.Routes.Mqtt}/{GlobalStatic.Routes.CHECK}")]
    public async Task<ResponseBaseModel> MqttConfigTestConnection(MqttConfigModel? mqtt_conf) => await _tools_service.TestMqttConnect(mqtt_conf);

    /// <summary>
    /// 
    /// </summary>
    [HttpPost($"{GlobalStatic.Routes.Mqtt}/{GlobalStatic.Routes.Mqtt}/{GlobalStatic.Routes.PUBLISH}")]
    public async Task<MqttPublishMessageResultModel> PublishMqttMessage(MqttPublishMessageModel message) => await _tools_service.PublishMqttMessage(message);

    /// <summary>
    /// Проверить токен TelegramBot
    /// </summary>
    [HttpPost($"{GlobalStatic.Routes.TelegramBot}/{GlobalStatic.Routes.CHECK}")]
    public async Task<DictionaryResponseModel> TelegramBotConfigTestConnection(TelegramBotConfigModel? telegram_bot_conf) => await _tools_service.TestTelegramBotConnect(telegram_bot_conf);
}