namespace SharedLib;

/// <summary>
/// 
/// </summary>
public class UserListGetModel
{
    /// <summary>
    /// 
    /// </summary>
    public int PageNum { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool IncludeDisabledUsers { get; set; }
}