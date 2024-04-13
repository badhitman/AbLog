////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using ab.context;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SharedLib;

namespace ServerLib;

/// <summary>
/// Хранение параметров в БД
/// </summary>
public class ParametersStorageLocalService(IDbContextFactory<ParametersContext> DbFactory) : IParametersStorageService
{
    #region Telegram Bot
    /// <inheritdoc/>
    public virtual async Task<ResponseBaseModel> SaveTelegramBotConfig(TelegramBotConfigModel connect_config, CancellationToken cancellationToken)
    {
        using ParametersContext _context = await DbFactory.CreateDbContextAsync(cancellationToken);
        ParametersStorageModelDB p = _context.SetStoredParameter(nameof(TelegramBotConfigModel), JsonConvert.SerializeObject(connect_config));
        ResponseBaseModel res = new();
        res.AddSuccess($"Данные успешно записаны в БД #{p.Id}");
        return res;
    }

    /// <inheritdoc/>
    public virtual async Task<TelegramBotConfigResponseModel> GetTelegramBotConfig(CancellationToken cancellationToken)
    {
        TelegramBotConfigResponseModel res = new();
        using ParametersContext _context = await DbFactory.CreateDbContextAsync(cancellationToken);
        string _telegramBotConfig = _context.GetStoredParameter(nameof(TelegramBotConfigModel), "").StoredValue;
        if (string.IsNullOrWhiteSpace(_telegramBotConfig))
        {
            res.AddWarning("Конфигурация не обнаружена");
            return res;
        }

        res.Conf = JsonConvert.DeserializeObject<TelegramBotConfigModel>(_telegramBotConfig) ?? new();
        return res;
    }
    #endregion

    #region Email
    /// <inheritdoc/>
    public virtual async Task<ResponseBaseModel> SaveEmailConfig(EmailConfigModel connect_config, CancellationToken cancellationToken)
    {
        if (!connect_config.IsConfigured)
            return ResponseBaseModel.CreateError("Настройки не заполнены. Заполните поля: email, login, pasword, host");

        using ParametersContext _context = await DbFactory.CreateDbContextAsync(cancellationToken);
        ParametersStorageModelDB p = _context.SetStoredParameter(nameof(EmailConfigModel), JsonConvert.SerializeObject(connect_config));
        ResponseBaseModel res = new();
        res.AddSuccess($"Данные успешно записаны в БД #{p.Id}");
        return res;
    }

    /// <inheritdoc/>
    public virtual async Task<EmailConfigResponseModel> GetEmailConfig(CancellationToken cancellationToken)
    {
        EmailConfigResponseModel res = new();
        using ParametersContext _context = await DbFactory.CreateDbContextAsync(cancellationToken);
        string _emailConfig = _context.GetStoredParameter(nameof(EmailConfigModel), "").StoredValue;
        if (string.IsNullOrWhiteSpace(_emailConfig))
        {
            res.AddWarning("Конфигурация не настроена");
            return res;
        }

        res.Conf = JsonConvert.DeserializeObject<EmailConfigModel>(_emailConfig) ?? new();

        return res;
    }
    #endregion

    #region Mqtt
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SaveMqttConfig(MqttConfigModel connect_config, CancellationToken cancellationToken)
    {
        using ParametersContext _context = await DbFactory.CreateDbContextAsync(cancellationToken);
        ParametersStorageModelDB p = _context.SetStoredParameter(nameof(MqttConfigModel), JsonConvert.SerializeObject(connect_config));
        ResponseBaseModel res = new();
        res.AddSuccess($"Данные успешно записаны в БД #{p.Id}");
        return res;
    }

    /// <inheritdoc/>
    public async Task<MqttConfigResponseModel> GetMqttConfig(CancellationToken cancellationToken)
    {
        MqttConfigResponseModel res = new();
        using ParametersContext _context = await DbFactory.CreateDbContextAsync(cancellationToken);
        string _mqttConfig = _context.GetStoredParameter(nameof(MqttConfigModel), "").StoredValue;
        if (string.IsNullOrWhiteSpace(_mqttConfig))
        {
            res.AddWarning("MQTT не настроен");
            return res;
        }
        res.Conf = JsonConvert.DeserializeObject<MqttConfigModel>(_mqttConfig);
        return res;
    }
    #endregion
}