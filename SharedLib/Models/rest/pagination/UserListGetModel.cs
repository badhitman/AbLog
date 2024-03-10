////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// User`s list get
/// </summary>
public class UserListGetModel
{
    /// <summary>
    /// Page num
    /// </summary>
    public int PageNum { get; set; }

    /// <summary>
    /// Page size
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Include disabled users
    /// </summary>
    public bool IncludeDisabledUsers { get; set; }
}