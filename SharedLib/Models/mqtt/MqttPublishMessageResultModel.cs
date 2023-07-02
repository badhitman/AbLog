namespace SharedLib;

/// <summary>
/// Результат публикации сообщения
/// </summary>
public class MqttPublishMessageResultModel : ResponseBaseModel
{
    /// <summary>
    /// Идентификатор пакета, который использовался для этой публикации.
    /// </summary>
    public ushort? PacketIdentifier { get; set; }

    /// <summary>
    /// Строка причины.
    /// </summary>
    public string ReasonString { get; set; } = default!;

    /// <summary>
    /// Свойства пользователя.
    /// В MQTT 5 пользовательские свойства представляют собой простые пары ключ-значение строки UTF-8, которые можно добавлять практически к каждому типу пакетов MQTT.
    /// </summary>
    public KeyValuePair<string, string>[] UserProperties { get; set; } = default!;
}