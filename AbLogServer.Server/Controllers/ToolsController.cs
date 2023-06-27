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
    /// ��������� MQTT ������
    /// </summary>
    [HttpGet($"{GlobalStatic.HttpRoutes.Mqtt}/{GlobalStatic.HttpRoutes.START}")]
    public async Task<ResponseBaseModel> StartMqtt() => await _toolss_service.StartMqtt();

    /// <summary>
    /// ���������� MQTT ������
    /// </summary>
    [HttpGet($"{GlobalStatic.HttpRoutes.Mqtt}/{GlobalStatic.HttpRoutes.STOP}")]
    public async Task<ResponseBaseModel> StopMqtt() => await _toolss_service.StopMqtt();

    /// <summary>
    /// �������� ������ MQTT ������
    /// </summary>
    [HttpGet($"{GlobalStatic.HttpRoutes.Mqtt}/{GlobalStatic.HttpRoutes.STATUS}")]
    public async Task<BoolResponseModel> StatusMqtt() => await _toolss_service.StatusMqtt();

    /// <summary>
    /// ��������� ����������� � Email (������������ imap+smtp)
    /// </summary>
    [HttpPost($"{GlobalStatic.HttpRoutes.Email}/{GlobalStatic.HttpRoutes.CHECK}")]
    public async Task<ResponseBaseModel> EmailConfigTestSmtpConnection(EmailConfigModel? email_conf) => await _toolss_service.TestEmailConnect(email_conf);

    /// <summary>
    /// ��������� ����������� � Mqtt
    /// </summary>
    [HttpPost($"{GlobalStatic.HttpRoutes.Mqtt}/{GlobalStatic.HttpRoutes.CHECK}")]
    public async Task<ResponseBaseModel> MqttConfigTestConnection(MqttConfigModel? mqtt_conf) => await _toolss_service.TestMqttConnect(mqtt_conf);

    /// <summary>
    /// 
    /// </summary>
    [HttpPost($"{GlobalStatic.HttpRoutes.Mqtt}/{GlobalStatic.HttpRoutes.Mqtt}/{GlobalStatic.HttpRoutes.PUBLISH}")]
    public async Task<MqttPublishMessageResultModel> PublishMqttMessage(MqttPublishMessageModel message) => await _toolss_service.PublishMqttMessage(message);
}