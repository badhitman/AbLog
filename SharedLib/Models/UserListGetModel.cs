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

/// <summary>
/// 
/// </summary>
public class UpdateUserModel
{
    /// <summary>
    /// 
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool IsDisabled { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool AllowAlerts { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool AllowHardwareControl { get; set; }
}