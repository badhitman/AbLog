////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Заполнение форм
/// </summary>
public static class FormFillingFlowsStatic
{
    /// <summary>
    /// Form filling flow`s
    /// </summary>
    public static readonly Dictionary<string, FormMetadataModel> FormFillingFlows = new()
    {
        { nameof(MqttConfigModel), new("Настройки MQTT", MatConfigFormProperties) }
    };

    /// <summary>
    /// MQTT config form properties
    /// </summary>
    public static PropertyFormSet[] MatConfigFormProperties =>
    [
        new PropertyFormSet(nameof(MqttConfigModel.Server), "Адрес сервера MQTT"),
        new PropertyFormSet(nameof(MqttConfigModel.Port), "Порт сервера (обычно 8883)"),
        new PropertyFormSet(nameof(MqttConfigModel.Username), "Логин для MQTT"),
        new PropertyFormSet(nameof(MqttConfigModel.Password), "Пароль для MQTT"),
        new PropertyFormSet(nameof(MqttConfigModel.Secret), "Секретная фраза для шифрования трафика"),
        new PropertyFormSet(nameof(MqttConfigModel.PrefixMqtt), "Префикс MQTT топиков")
    ];
}