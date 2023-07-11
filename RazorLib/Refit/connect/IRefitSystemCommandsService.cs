////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using SharedLib;
using Refit;

namespace BlazorLib;

/// <summary>
/// Работа с бэком: System commands
/// </summary>
[Headers("Content-Type: application/json")]
public interface IRefitSystemCommandsService
{
    /// <summary>
    /// Команды (все)
    /// </summary>
    [Get($"/api/{GlobalStatic.Routes.System}/{GlobalStatic.Routes.Commands}/{GlobalStatic.Routes.LIST}")]
    public Task<ApiResponse<SystemCommandsResponseModel>> CommandsGetAll(CancellationToken cancellation_token = default);

    /// <summary>
    /// 
    /// </summary>
    [Post($"/api/{GlobalStatic.Routes.System}/{GlobalStatic.Routes.Commands}/{GlobalStatic.Routes.UPDATE}")]
    public Task<ApiResponse<ResponseBaseModel>> CommandUpdateOrCreate(SystemCommandModelDB comm, CancellationToken cancellation_token = default);

    /// <summary>
    /// 
    /// </summary>
    [Delete($"/api/{GlobalStatic.Routes.System}/{GlobalStatic.Routes.Commands}/{GlobalStatic.Routes.DELETE}/{{comm_id}}")]
    public Task<ApiResponse<ResponseBaseModel>> CommandDelete(int comm_id, CancellationToken cancellation_token = default);

    /// <summary>
    /// 
    /// </summary>
    [Put($"/api/{GlobalStatic.Routes.System}/{GlobalStatic.Routes.Commands}/{{comm_id}}")]
    public Task<ApiResponse<ResponseBaseModel>> CommandRun(int comm_id, CancellationToken cancellation_token = default);
}