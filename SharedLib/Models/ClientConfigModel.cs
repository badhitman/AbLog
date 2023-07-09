////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// 
/// </summary>
public class ClientConfigModel
{
    /// <summary>
    /// 
    /// </summary>
    public Dictionary<string, string> MetadataInput { get; set; } = default!;

    /// <summary>
    /// 
    /// </summary>
    public Dictionary<string, IEnumerable<string>> MetadataPage { get; set; } = default!;
}
