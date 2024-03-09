////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// 
/// </summary>
public class EntriyResponseModel : ResponseBaseModel
{
    /// <summary>
    /// Перечень базовых элеметнов [Entry]
    /// </summary>
    public EntryModel? Entry { get; set; }
}