////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// 
/// </summary>
public class UserFormModelDb : EntryModel
{
    /// <summary>
    /// 
    /// </summary>
    public UserModelDB? OwnerUser { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int OwnerUserId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public UserFormPropertyModelDb[]? Properties { get; set; }
}
