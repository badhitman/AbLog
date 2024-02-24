﻿using Microsoft.Extensions.DependencyInjection;
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
    public TelegramBotFormFillingServive(IServiceProvider _services, ILogger<TelegramBotFormFillingServive> logger)
    {
        using IServiceScope scope = _services.CreateScope();
        _botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
        _logger = logger;
    }
}