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
    public required string Code { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Allowed values
    /// </summary>
    public string[] AllowedValues { get; set; } = [];
}