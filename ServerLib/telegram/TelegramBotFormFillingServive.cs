using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace ServerLib;

/// <summary>
/// Заполнение формы данными через TelegramBot
/// </summary>
public class TelegramBotFormFillingServive : TelegramBotFormFillingServiveAbstract
{
    /// <summary>
    /// Заполнение формы данными через TelegramBot
    /// </summary>
    public TelegramBotFormFillingServive(IServiceProvider _services, ILogger<TelegramBotFormFillingServive> logger)
    {
        using IServiceScope scope = _services.CreateScope();
        BotClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
        Logger = logger;
    }
}