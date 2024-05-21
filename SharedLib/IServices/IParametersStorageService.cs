////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Хранение параметров в БД
/// </summary>
public interface IParametersStorageService
{
    #region TelegramBot (удалённое хранение: на сервере)

    /// <summary>
    /// Сохранить конфигурацию TelegramBot токена
    /// </summary>
    public Task<ResponseBaseModel> SaveTelegramBotConfig(TelegramBotConfigModel connect_config, CancellationToken cancellationToken);

    /// <summary>
    /// Получить конфигурацию TelegramBot токена
    /// </summary>
    public Task<TResponseModel<TelegramBotConfigModel>> GetTelegramBotConfig(CancellationToken cancellationToken);

    #endregion

    #region Email (удалённое хранение: на сервере)

    /// <summary>
    /// Сохранить конфигурацию Email подключения
    /// </summary>
    public Task<ResponseBaseModel> SaveEmailConfig(EmailConfigModel connect_config, CancellationToken cancellationToken);

    /// <summary>
    /// Получить конфигурацию Email подключения
    /// </summary>
    public Task<TResponseModel<EmailConfigModel>> GetEmailConfig(CancellationToken cancellationToken);

    #endregion

    #region MQTT (локальное хранение)

    /// <summary>
    /// Сохранить настройки MQTT
    /// </summary>
    public Task<ResponseBaseModel> SaveMqttConfig(MqttConfigModel conf, CancellationToken cancellationToken);

    /// <summary>
    /// Получить настройки MQTT
    /// </summary>
    public Task<TResponseModel<MqttConfigModel>> GetMqttConfig(CancellationToken cancellationToken);

    #endregion
}