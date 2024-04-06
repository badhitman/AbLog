////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Entriy
/// </summary>
public class EntriyResponseModel : ResponseBaseModel
{
    /// <summary>
    /// Перечень базовых элеметнов [Entry]
    /// </summary>
    public EntryModel? Entry { get; set; }
}