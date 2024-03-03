namespace SharedLib;

/// <summary>
/// 
/// </summary>
public interface INotifyService
{
    /// <summary>
    /// 
    /// </summary>
    public delegate void AccountHandler(UserResponseModel user);

    /// <summary>
    /// 
    /// </summary>
    public event AccountHandler? Notify;


    /// <summary>
    /// 
    /// </summary>
    public void CheckTelegramUser(UserResponseModel user);
}
