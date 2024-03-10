////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Form context
/// </summary>
public class FormContextModel
{
    /// <summary>
    /// Action
    /// </summary>
    public Uri FormAction { get; set; } = default!;

    /// <summary>
    /// Data context
    /// </summary>
    public Dictionary<string, string> DataContext { get; set; } = [];
}