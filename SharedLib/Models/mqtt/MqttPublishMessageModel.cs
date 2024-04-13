////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// MQTT publish message
/// </summary>
public class MqttPublishMessageModel(byte[] payload, string[] topics)
{

    /// <inheritdoc/>
    public byte[] Payload { get; set; } = payload;

    /// <inheritdoc/>
    public string[] Topics { get; set; } = topics;

    /// <inheritdoc/>
    public bool RetainFlag { get; set; } = false;

    /// <inheritdoc/>
    public string[]? ResponseTopics { get; set; } = null;

    /// <inheritdoc/>
    public IEnumerable<KeyValuePair<string, string>>? UserProperties { get; set; } = null;

    /// <inheritdoc/>
    public byte[]? CorrelationData { get; set; } = null;
}