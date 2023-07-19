////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// 
/// </summary>
public class UserFormPropertyModelDb : EntryModel
{
    /// <summary>
    /// 
    /// </summary>
    public UserFormModelDb? OwnerForm { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int OwnerFormId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? PropValue { get; set; }
}