////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Form property
/// </summary>
public class FormPropertyModel
{
    /// <inheritdoc/>
    public required string Code { get; set; }

    /// <inheritdoc/>
    public required string Name { get; set; }

    /// <summary>
    /// Allowed values
    /// </summary>
    public string[] AllowedValues { get; set; } = [];
}