////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Конфигурация клиента
/// </summary>
public class ClientConfigModel
{
    /// <summary>
    /// Metadata input
    /// </summary>
    public Dictionary<string, string> MetadataInput { get; set; } = default!;

    /// <summary>
    /// Metadata page
    /// </summary>
    public Dictionary<string, IEnumerable<string>> MetadataPage { get; set; } = default!;
}
