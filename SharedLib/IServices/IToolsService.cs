﻿namespace SharedLib;

/// <summary>
/// Tools
/// </summary>
public interface IToolsService
{
    /// <summary>
    /// Запустить MQTT службу
    /// </summary>
    public Task<ResponseBaseModel> StartMqtt();

    /// <summary>
    /// Остановить MQTT службу
    /// </summary>
    public Task<ResponseBaseModel> StopMqtt();

    /// <summary>
    /// Получить статус MQTT службы
    /// </summary>
    public Task<BoolResponseModel> StatusMqtt();

    /// <summary>
    /// Проверить подключение MQTT
    /// </summary>
    public Task<ResponseBaseModel> TestMqttConnect(MqttConfigModel? conf = null);

    /// <summary>
    /// Тест Email подключения (SMTP)
    /// </summary>
    public Task<ResponseBaseModel> TestEmailConnect(EmailConfigModel? conf = null);

    /// <summary>
    /// 
    /// </summary>
    public Task<MqttPublishMessageResultModel> PublishMqttMessage(MqttPublishMessageModel message);
}