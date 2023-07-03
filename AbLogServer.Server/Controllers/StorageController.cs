using Microsoft.AspNetCore.Mvc;
using SharedLib;

namespace ABLog;

/// <summary>
/// 
/// </summary>
[ApiController]
[Route("/api/[controller]")]
public class StorageController : ControllerBase
{
    private readonly IParametersStorageService _parameters_storage;
    private readonly ILogger<StorageController> _logger;

    /// <summary>
    /// 
    /// </summary>
    public StorageController(ILogger<StorageController> logger, IParametersStorageService parameters_storage)
    {
        _logger = logger;
        _parameters_storage = parameters_storage;
    }

    #region Email

    /// <summary>
    /// Получить конфигурацию Email (imap+smtp)
    /// </summary>
    [HttpGet($"{GlobalStatic.Routes.Email}/{GlobalStatic.Routes.GET}")]
    public async Task<EmailConfigResponseModel> EmailConfigGet() => await _parameters_storage.GetEmailConfig();

    /// <summary>
    /// Сохранить конфигурацию Email (imap+smtp)
    /// </summary>
    [HttpPost($"{GlobalStatic.Routes.Email}/{GlobalStatic.Routes.UPDATE}")]
    public async Task<ResponseBaseModel> EmailConfigSave(EmailConfigModel e_conf) => await _parameters_storage.SaveEmailConfig(e_conf);

    #endregion

    #region MQTT

    /// <summary>
    /// Получить конфигурацию Mqtt
    /// </summary>
    [HttpGet($"{GlobalStatic.Routes.Mqtt}/{GlobalStatic.Routes.GET}")]
    public async Task<MqttConfigResponseModel> MqttConfigGet() => await _parameters_storage.GetMqttConfig();

    /// <summary>
    /// Сохранить конфигурацию Mqtt (imap+smtp)
    /// </summary>
    [HttpPost($"{GlobalStatic.Routes.Mqtt}/{GlobalStatic.Routes.UPDATE}")]
    public async Task<ResponseBaseModel> MqttConfigSave(MqttConfigModel e_conf)
    {
        return await _parameters_storage.SaveMqttConfig(e_conf);
    }

    #endregion
}