////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Конфигурация службы
/// </summary>
public class ConfigsAppModel
{
    /// <summary>
    /// Порт на котором будет работать HttpListener
    /// </summary>
    public int HttpListenerPort { get; set; } = 8181;

    /// <summary>
    /// Порт на котором будет работать MQTT сервер
    /// </summary>
    public int MqttListenerPort { get; set; } = 8282;

    /// <summary>
    /// Разрешённое количество потоков для обслуживания http запросов
    /// </summary>
    public int MaxThreadsHttpListener { get; set; } = 32;

    /// <summary>
    /// http префиксы для listener
    /// </summary>
    public string[] HttpListenerPrefixes { get; set; } = [];
}