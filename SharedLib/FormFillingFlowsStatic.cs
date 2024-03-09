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
    /// 
    /// </summary>
    public static Dictionary<string, FormMetadataModel> FormFillingFlows = new()
    {
        { nameof(MqttConfigModel), new("Настройки MQTT", MqttConfigFormPropertyes) }
    };

    /// <summary>
    /// 
    /// </summary>
    public static PropertyFormSet[] MqttConfigFormPropertyes =>
    [
        new PropertyFormSet(nameof(MqttConfigModel.Server), "Адрес сервера MQTT"),
        new PropertyFormSet(nameof(MqttConfigModel.Port), "Порт сервера (обычно 8883)"),
        new PropertyFormSet(nameof(MqttConfigModel.Username), "Логин для MQTT"),
        new PropertyFormSet(nameof(MqttConfigModel.Password), "Пароль для MQTT"),
        new PropertyFormSet(nameof(MqttConfigModel.Secret), "Секретная фраза для шифрования трафика"),
        new PropertyFormSet(nameof(MqttConfigModel.PrefixMqtt), "Префикс MQTT топиков")
    ];
}

/// <summary>
/// Форма
/// </summary>
/// <param name="Title">Заголовок формы</param>
/// <param name="MqttConfigFormPropertyes">Поля формы</param>
public record struct FormMetadataModel(string Title, PropertyFormSet[] MqttConfigFormPropertyes);

/// <summary>
/// Поле формы для заполнения
/// </summary>
/// <param name="Code">Код поля формы</param>
/// <param name="Title">Название поля для пользователя</param>
/// <param name="AllowedValues">Доступные значения (по желанию). Если в этом перечне есть элементы, то значения в это поле будут ограничены этим списком</param>
public record struct PropertyFormSet(string Code, string Title, string[]? AllowedValues = null);