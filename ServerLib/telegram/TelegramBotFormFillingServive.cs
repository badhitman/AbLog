using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace ServerLib;

/// <summary>
/// 
/// </summary>
public class TelegramBotFormFillingServive : TelegramBotFormFillingServiveAbstract
{
    /// <summary>
    /// 
    /// </summary>
    public TelegramBotFormFillingServive(ITelegramBotClient botClient, ILogger<TelegramBotFormFillingServive> logger)
    {
        _botClient = botClient;
        _logger = logger;
    }
}