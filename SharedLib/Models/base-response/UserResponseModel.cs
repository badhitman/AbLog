////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Пользователь
/// </summary>
public class UserResponseModel : ResponseBaseModel
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public UserModelDB? User { get; set; }
}