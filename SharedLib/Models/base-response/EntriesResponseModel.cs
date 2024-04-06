////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Перечень базовых элеметнов [Entry]
/// </summary>
public class EntriesResponseModel : ResponseBaseModel
{
    /// <summary>
    /// Перечень базовых элеметнов [Entry]
    /// </summary>
    public IEnumerable<EntryModel>? Entries { get; set; }
}