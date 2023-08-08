using Microsoft.Extensions.Logging;
using Telegram.Bot.Abstract;

namespace Telegram.Bot.Services;

/// <summary>
/// 
/// </summary>
public class ReceiverService : ReceiverServiceBase<UpdateHandler>
{
    /// <summary>
    /// 
    /// </summary>
    public ReceiverService(
        ITelegramBotClient botClient,
        UpdateHandler updateHandler,
        ILogger<ReceiverServiceBase<UpdateHandler>> logger)
        : base(botClient, updateHandler, logger)
    {
    }
}
