////////////////////////////////////////////////
// � https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Mvc;
using SharedLib;

namespace ABLogWeb;

/// <summary>
/// �������� ������
/// </summary>
[Route("/api/[controller]"), ApiController]
public class StorageController(IParametersStorageService parameters_storage) : ControllerBase
{
    #region TelegramBot
    /// <summary>
    /// �������� ������������ TelegramBot
    /// </summary>
    [HttpGet($"{GlobalStatic.Routes.TelegramBot}/{GlobalStatic.Routes.CONFIG}/{GlobalStatic.Routes.GET}")]
    public async Task<TelegramBotConfigResponseModel> TelegramBotConfigGet()
        => await parameters_storage.GetTelegramBotConfig();

    /// <summary>
    /// ��������� ������������ TelegramBot
    /// </summary>
    [HttpPost($"{GlobalStatic.Routes.TelegramBot}/{GlobalStatic.Routes.CONFIG}/{GlobalStatic.Routes.UPDATE}")]
    public async Task<ResponseBaseModel> TelegramBotConfigSave(TelegramBotConfigModel t_conf)
        => await parameters_storage.SaveTelegramBotConfig(t_conf);
    #endregion

    #region Email
    /// <summary>
    /// �������� ������������ Email (smtp)
    /// </summary>
    [HttpGet($"{GlobalStatic.Routes.Email}/{GlobalStatic.Routes.CONFIG}/{GlobalStatic.Routes.GET}")]
    public async Task<EmailConfigResponseModel> EmailConfigGet()
        => await parameters_storage.GetEmailConfig();

    /// <summary>
    /// ��������� ������������ Email (smtp)
    /// </summary>
    [HttpPost($"{GlobalStatic.Routes.Email}/{GlobalStatic.Routes.CONFIG}/{GlobalStatic.Routes.UPDATE}")]
    public async Task<ResponseBaseModel> EmailConfigSave(EmailConfigModel e_conf)
        => await parameters_storage.SaveEmailConfig(e_conf);
    #endregion

    #region MQTT
    /// <summary>
    /// �������� ������������ Mqtt
    /// </summary>
    [HttpGet($"{GlobalStatic.Routes.Mqtt}/{GlobalStatic.Routes.CONFIG}/{GlobalStatic.Routes.GET}")]
    public async Task<MqttConfigResponseModel> MqttConfigGet()
        => await parameters_storage.GetMqttConfig();

    /// <summary>
    /// ��������� ������������ Mqtt
    /// </summary>
    [HttpPost($"{GlobalStatic.Routes.Mqtt}/{GlobalStatic.Routes.CONFIG}/{GlobalStatic.Routes.UPDATE}")]
    public async Task<ResponseBaseModel> MqttConfigSave(MqttConfigModel e_conf)
        => await parameters_storage.SaveMqttConfig(e_conf);
    #endregion
}