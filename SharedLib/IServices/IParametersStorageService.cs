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
    public Task<ResponseBaseModel> SaveTelegramBotConfig(TelegramBotConfigModel connect_config);

    /// <summary>
    /// Получить конфигурацию TelegramBot токена
    /// </summary>
    public Task<TelegramBotConfigResponseModel> GetTelegramBotConfig();

    #endregion

    #region Email (удалённое хранение: на сервере)

    /// <summary>
    /// Сохранить конфигурацию Email подключения
    /// </summary>
    public Task<ResponseBaseModel> SaveEmailConfig(EmailConfigModel connect_config);

    /// <summary>
    /// Получить конфигурацию Email подключения
    /// </summary>
    public Task<EmailConfigResponseModel> GetEmailConfig();

    #endregion

    #region MQTT (локальное хранение)

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