////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// 
/// </summary>
public class FormPropertyModel
{
    /// <summary>
    /// 
    /// </summary>
    public string Code { get; set; } = default!;

    /// <summary>
    /// 
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// 
    /// </summary>
    public string[] AllowedValues { get; set; } = Array.Empty<string>();
}