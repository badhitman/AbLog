﻿////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Newtonsoft.Json;
using ab.context;
using SharedLib;
using MQTTnet;

namespace ServerLib;

/// <summary>
/// Хранение параметров IMqttClient
/// </summary>
public class ParametersStorageLocalService : IParametersStorageService
{
    //readonly MqttFactory _mqtt_fact;

    /// <summary>
    /// Хранение параметров IMqttClient
    /// </summary>
    public ParametersStorageLocalService()
    {
        // _mqtt_fact = mqtt_fact;
    }

    #region Telegram Bot

    /// <inheritdoc/>
    public virtual Task<ResponseBaseModel> SaveTelegramBotConfig(TelegramBotConfigModel connect_config)
    {
        using ParametersContext _context = new();
        ParametersStorageModelDB p = _context.SetStoredParameter(nameof(TelegramBotConfigModel), JsonConvert.SerializeObject(connect_config));
        ResponseBaseModel res = new();
        res.AddSuccess($"Данные успешно записаны в БД #{p.Id}");
        return Task.FromResult(res);
    }

    /// <inheritdoc/>
    public virtual Task<TelegramBotConfigResponseModel> GetTelegramBotConfig()
    {
        TelegramBotConfigResponseModel res = new();
        using ParametersContext _context = new();
        string _telegramBotConfig = _context.GetStoredParameter(nameof(TelegramBotConfigModel), "").StoredValue;
        if (string.IsNullOrWhiteSpace(_telegramBotConfig))
        {
            res.AddWarning("Конфигурация не обнаружена");
            return Task.FromResult(res);
        }

        res.Conf = JsonConvert.DeserializeObject<TelegramBotConfigModel>(_telegramBotConfig) ?? new();

        return Task.FromResult(res);
    }

    #endregion

    #region Email

    /// <inheritdoc/>
    public Task<ResponseBaseModel> SaveEmailConfig(EmailConfigModel connect_config)
    {
        using ParametersContext _context = new();
        ParametersStorageModelDB p = _context.SetStoredParameter(nameof(EmailConfigModel), JsonConvert.SerializeObject(connect_config));
        ResponseBaseModel res = new();
        res.AddSuccess($"Данные успешно записаны в БД #{p.Id}");
        return Task.FromResult(res);
    }

    /// <inheritdoc/>
    public Task<EmailConfigResponseModel> GetEmailConfig()
    {
        EmailConfigResponseModel res = new();
        using ParametersContext _context = new();
        string _emailConfig = _context.GetStoredParameter(nameof(EmailConfigModel), "").StoredValue;
        if (string.IsNullOrWhiteSpace(_emailConfig))
            return Task.FromResult(res);

        res.Conf = JsonConvert.DeserializeObject<EmailConfigModel>(_emailConfig) ?? new();

        return Task.FromResult(res);
    }

    #endregion

    #region Mqtt

    /// <inheritdoc/>
    public Task<ResponseBaseModel> SaveMqttConfig(MqttConfigModel connect_config)
    {
        using ParametersContext _context = new();
        ParametersStorageModelDB p = _context.SetStoredParameter(nameof(MqttConfigModel), JsonConvert.SerializeObject(connect_config));
        ResponseBaseModel res = new();
        res.AddSuccess($"Данные успешно записаны в БД #{p.Id}");
        return Task.FromResult(res);
    }

    /// <inheritdoc/>
    public Task<MqttConfigResponseModel> GetMqttConfig()
    {
        MqttConfigResponseModel res = new();
        using ParametersContext _context = new();
        string _mqttConfig = _context.GetStoredParameter(nameof(MqttConfigModel), "").StoredValue;
        if (string.IsNullOrWhiteSpace(_mqttConfig))
            return Task.FromResult(res);

        res.Conf = JsonConvert.DeserializeObject<MqttConfigModel>(_mqttConfig) ?? new();

        return Task.FromResult(res);
    }

    #endregion
}