////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Form property
/// </summary>
public class FormPropertyModel
{
    /// <summary>
    /// Code
    /// </summary>
    public string Code { get; set; } = default!;

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Allowed values
    /// </summary>
    public string[] AllowedValues { get; set; } = [];
}