////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// 
/// </summary>
public class UserFormModelDb
{
    /// <summary>
    /// Ключ/идентификатор
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string FormMapCode { get; set; } = default!;

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
    public List<UserFormPropertyModelDb>? Properties { get; set; }
}