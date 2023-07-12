using ab.context;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SharedLib;
using System.Diagnostics;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Services;

/// <summary>
/// 
/// </summary>
public class UpdateHandler : IUpdateHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<UpdateHandler> _logger;

    /// <summary>
    /// 
    /// </summary>
    public UpdateHandler(ITelegramBotClient botClient, ILogger<UpdateHandler> logger)
    {
        _botClient = botClient;
        _logger = logger;
    }

    /// <summary>
    /// 
    /// </summary>
    public async Task HandleUpdateAsync(ITelegramBotClient _, Update update, CancellationToken cancellationToken)
    {
        var handler = update switch
        {
            // UpdateType.Unknown:
            // UpdateType.ChannelPost:
            // UpdateType.EditedChannelPost:
            // UpdateType.ShippingQuery:
            // UpdateType.PreCheckoutQuery:
            // UpdateType.Poll:
            { Message: { } message } => BotOnMessageReceived(message, cancellationToken),
            //{ EditedMessage: { } message } => BotOnMessageReceived(message, cancellationToken),
            { CallbackQuery: { } callbackQuery } => BotOnCallbackQueryReceived(callbackQuery, cancellationToken),
            _ => UnknownUpdateHandlerAsync(update, cancellationToken)
        };

        await handler;
    }

    private async Task BotOnMessageReceived(Message message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Receive message type: {MessageType}", message.Type);
        if (message.Text is not { } messageText)
            return;

        ResponseBaseModel check_user = CheckTelegramUser(message.From);

        var action = messageText.Split(' ')[0] switch
        {
            "/throw" => FailingHandler(_botClient, message, cancellationToken),
            _ => Usage(_botClient, message, cancellationToken)
        };
        Message sentMessage = await action;
        _logger.LogInformation("The message was sent with id: {SentMessageId}", sentMessage.MessageId);

        static async Task<Message> Usage(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            string usage = "Бот-обормот";

            using ServerContext _context = new();
            InlineKeyboardMarkup inlineKeyboard;
            lock (ServerContext.DbLocker)
            {
                inlineKeyboard = new(
                    _context.SystemCommands
                    .Where(x => !x.IsDisabled)
                    .AsEnumerable()
                    .Select(x => new InlineKeyboardButton[] {
                        InlineKeyboardButton.WithCallbackData(x.Name, x.Id.ToString())
                    }).ToArray()
                );
            }

            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: usage,
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }

#pragma warning disable RCS1163 // Unused parameter.
#pragma warning disable IDE0060 // Remove unused parameter
        static Task<Message> FailingHandler(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            throw new IndexOutOfRangeException();
        }
#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore RCS1163 // Unused parameter.
    }

    private ResponseBaseModel CheckTelegramUser(User? from)
    {
        ResponseBaseModel res = new();
        if (from is null)
        {
            res.AddError("from is null. error {5513E9FE-75DB-4C9F-92A3-7537072AD3CD}");
            return res;
        }

        using ServerContext context = new();
        lock (ServerContext.DbLocker)
        {
            UserModelDB? user_db = context.Users.FirstOrDefault(x => x.TelegramId == from.Id);
            if (user_db is null)
            {
                user_db = new()
                {
                    FirstName = from.FirstName,
                    LastName = from.LastName,
                    Name = from.Username ?? "",
                    TelegramId = from.Id
                };
                context.Add(user_db);
                context.SaveChanges();
            }
        }


        return res;
    }

    // Process Inline Keyboard callback data
    private async Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received inline keyboard callback from: {CallbackQueryId}", callbackQuery.Id);

        ResponseBaseModel check_user = CheckTelegramUser(callbackQuery.Message?.From);

        await _botClient.AnswerCallbackQueryAsync(
            callbackQueryId: callbackQuery.Id,
            text: $"Received {callbackQuery.Data}",
            cancellationToken: cancellationToken);

        switch (callbackQuery.Data)
        {
            case $"{GlobalStatic.Routes.Mqtt}/{GlobalStatic.Routes.STATUS}":
                break;
            case $"{GlobalStatic.Routes.Mqtt}/{GlobalStatic.Routes.UPDATE}":
                break;
            case $"{GlobalStatic.Routes.System}/{GlobalStatic.Routes.UPDATE}":
                Process? proc = Process.Start(new ProcessStartInfo() { FileName = "/usr/bin/sudo", Arguments = "reboot" });
                await _botClient.SendTextMessageAsync(
            chatId: callbackQuery.Message!.Chat.Id,
            text: $"proc: {JsonConvert.SerializeObject(proc)}",
            cancellationToken: cancellationToken);
                return;
        }

        await _botClient.SendTextMessageAsync(
            chatId: callbackQuery.Message!.Chat.Id,
            text: $"Received {callbackQuery.Data}",
            cancellationToken: cancellationToken);
    }

#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable RCS1163 // Unused parameter.
    private Task UnknownUpdateHandlerAsync(Update update, CancellationToken cancellationToken)
#pragma warning restore RCS1163 // Unused parameter.
#pragma warning restore IDE0060 // Remove unused parameter
    {
        _logger.LogInformation("Unknown update type: {UpdateType}", update.Type);
        return Task.CompletedTask;
    }

    /// <summary>
    /// 
    /// </summary>
    public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        _logger.LogInformation("HandleError: {ErrorMessage}", ErrorMessage);

        // Cooldown in case of network connection error
        if (exception is RequestException)
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
    }
}