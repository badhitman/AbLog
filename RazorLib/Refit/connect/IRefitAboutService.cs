////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using SharedLib;
using Refit;

namespace RazorLib;

/// <summary>
/// О программе
/// </summary>
[Headers("Content-Type: application/json")]
public interface IRefitAboutService
{
    /// <summary>
    /// Получить все доступные камеры
    /// </summary>
    [Get($"/api/{GlobalStatic.Routes.About}/{GlobalStatic.Routes.GET}")]
    public Task<ApiResponse<EntriesSortingResponseModel>> About();
}