////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Form map
/// </summary>
public class FormMapModel
{
    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Properties
    /// </summary>
    public FormPropertyModel[] Properties { get; set; } = default!;
}
