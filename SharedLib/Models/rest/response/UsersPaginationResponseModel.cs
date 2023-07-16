////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Пользователи
/// </summary>
public class UsersPaginationResponseModel : PaginationResponseModel
{
    /// <summary>
    /// Пользователи
    /// </summary>
    public IEnumerable<UserModelDB>? Users { get; set; }
}