////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// 
/// </summary>
public class MqttPublishMessageModel
{
    /// <summary>
    /// 
    /// </summary>
    public MqttPublishMessageModel(byte[] payload, string[] topics)
    {
        Payload = payload;
        Topics = topics;
    }

    /// <summary>
    /// 
    /// </summary>
    public byte[] Payload { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string[] Topics { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool RetainFlag { get; set; } = false;

    /// <summary>
    /// 
    /// </summary>
    public string[]? ResponseTopics { get; set; } = null;

    /// <summary>
    /// 
    /// </summary>
    public IEnumerable<KeyValuePair<string, string>>? UserProperties { get; set; } = null;

    /// <summary>
    /// 
    /// </summary>
    public byte[]? CorrelationData { get; set; } = null;
}