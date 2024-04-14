using Microsoft.Extensions.Logging;
using Telegram.Bot.Abstract;

namespace Telegram.Bot.Services;

/// <summary>
/// Polling background service and Receiver implementation
/// </summary>
/// <remarks>
/// A background service consuming a scoped service.
/// See more: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services#consuming-a-scoped-service-in-a-background-task
/// </remarks>
/// <remarks>
/// 
/// </remarks>
/// <param name="serviceProvider"></param>
/// <param name="logger"></param>
public class PollingService(IServiceProvider serviceProvider, ILogger<PollingService> logger) : PollingServiceBase<ReceiverService>(serviceProvider, logger)
{
}