////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Form map
/// </summary>
public class FormMapModel
{
    /// <inheritdoc/>
    public required string Name { get; set; }

    /// <inheritdoc/>
    public required FormPropertyModel[] Properties { get; set; }
}
