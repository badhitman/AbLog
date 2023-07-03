﻿using SharedLib;
using Refit;

namespace BlazorLib;

/// <summary>
/// Работа с бэком: Storage
/// </summary>
[Headers("Content-Type: application/json")]
public interface IRefitStorageService
{
    #region Email

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

    #region Mqtt

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
}