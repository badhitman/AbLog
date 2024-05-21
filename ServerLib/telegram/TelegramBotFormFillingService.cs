using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace ServerLib;

/// <summary>
/// Заполнение формы данными через TelegramBot
/// </summary>
public class TelegramBotFormFillingService : TelegramBotFormFillingServiceAbstract
{
    /// <summary>
    /// Заполнение формы данными через TelegramBot
    /// </summary>
    public TelegramBotFormFillingService(IServiceProvider _services, ILogger<TelegramBotFormFillingService> logger)
    {
        using IServiceScope scope = _services.CreateScope();
        BotClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
        Logger = logger;
    }
}