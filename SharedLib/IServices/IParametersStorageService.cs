﻿namespace SharedLib;

/// <summary>
/// Хранение параметров в БД
/// </summary>
public interface IParametersStorageService
{
    #region Email

    /// <summary>
    /// Сохранить конфигурацию Email подключения
    /// </summary>
    public Task<ResponseBaseModel> SaveEmailConfig(EmailConfigModel connect_config);

    /// <summary>
    /// Получить конфигурацию Email подключения
    /// </summary>
    public Task<EmailConfigResponseModel> GetEmailConfig();

    #endregion

    #region MQTT

    /// <summary>
    /// Сохранить настройки MQTT
    /// </summary>
    public Task<ResponseBaseModel> SaveMqttConfig(MqttConfigModel conf);

    /// <summary>
    /// Получить настройки MQTT
    /// </summary>
    public Task<MqttConfigResponseModel> GetMqttConfig();

    #endregion
}