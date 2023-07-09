////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using SharedLib;
using Refit;

namespace BlazorLib;

/// <summary>
/// Работа с бэком: Storage
/// </summary>
[Headers("Content-Type: application/json")]
public interface IRefitStorageService
{
    #region Email (удалённое хранение: на сервере)

    /// <summary>
    /// Получить конфигурацию Email (imap+smtp)
    /// </summary>
    [Get($"/api/{GlobalStatic.Routes.Storage}/{GlobalStatic.Routes.Email}/{GlobalStatic.Routes.GET}")]
    public Task<ApiResponse<EmailConfigResponseModel>> EmailConfigGet();

    /// <summary>
    /// Сохранить конфигурацию Email (imap+smtp)
    /// </summary>
    [Post($"/api/{GlobalStatic.Routes.Storage}/{GlobalStatic.Routes.Email}/{GlobalStatic.Routes.UPDATE}")]
    public Task<ApiResponse<ResponseBaseModel>> EmailConfigSave(EmailConfigModel email_conf);

    #endregion

    #region Mqtt (локальное хранение конфигурации)

    /// <summary>
    /// Получить конфигурацию Mqtt
    /// </summary>
    [Get($"/api/{GlobalStatic.Routes.Storage}/{GlobalStatic.Routes.Mqtt}/{GlobalStatic.Routes.GET}")]
    public Task<ApiResponse<MqttConfigResponseModel>> MqttConfigGet();

    /// <summary>
    /// Сохранить конфигурацию Mqtt
    /// </summary>
    [Post($"/api/{GlobalStatic.Routes.Storage}/{GlobalStatic.Routes.Mqtt}/{GlobalStatic.Routes.UPDATE}")]
    public Task<ApiResponse<ResponseBaseModel>> MqttConfigSave(MqttConfigModel mqtt_conf);

    #endregion

    #region TelegramBot (локальное хранение конфигурации)

    /// <summary>
    /// Получить конфигурацию TelegramBot
    /// </summary>
    [Get($"/api/{GlobalStatic.Routes.Storage}/{GlobalStatic.Routes.TelegramBot}/{GlobalStatic.Routes.GET}")]
    public Task<ApiResponse<TelegramBotConfigResponseModel>> TelegramBotConfigGet();

    /// <summary>
    /// Сохранить конфигурацию TelegramBot
    /// </summary>
    [Post($"/api/{GlobalStatic.Routes.Storage}/{GlobalStatic.Routes.TelegramBot}/{GlobalStatic.Routes.UPDATE}")]
    public Task<ApiResponse<ResponseBaseModel>> TelegramBotConfigSave(TelegramBotConfigModel mqtt_conf);

    #endregion

}