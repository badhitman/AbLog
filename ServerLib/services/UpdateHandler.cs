using Telegram.Bot.Types.ReplyMarkups;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using System.Diagnostics;
using Telegram.Bot.Types;
using ab.context;
using SharedLib;
using System.Text.RegularExpressions;
using System.Threading;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Services;

/// <summary>
/// 
/// </summary>
public class UpdateHandler : IUpdateHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<UpdateHandler> _logger;

    const string SystemCommandPrefix = "sys:";

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
            //{ EditedMessage: { } message } => BotOnMessageReceived(message, cancellationToken),
            { Message: { } message } => BotOnMessageReceived(message, cancellationToken),
            { CallbackQuery: { } callbackQuery } => BotOnCallbackQueryReceived(callbackQuery, cancellationToken),
            _ => UnknownUpdateHandlerAsync(update, cancellationToken)
        };

        await handler;
    }

    private async Task BotOnMessageReceived(Message message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Receive message type: {MessageType}", message.Type);
        if (message.Text is not { } messageText || message.From?.IsBot == true)
            return;

        await _botClient.SendChatActionAsync(
               chatId: message.Chat.Id,
               chatAction: ChatAction.Typing,
               cancellationToken: cancellationToken);

        UserResponseModel check_user = CheckTelegramUser(message.From);
        if (!check_user.IsSuccess || check_user.User is null || check_user.User.IsDisabled == true)
            return;

        Task<Message> action = messageText.Split(' ')[0] switch
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
            List<InlineKeyboardButton[]> kb_rows;
            lock (ServerContext.DbLocker)
            {
                kb_rows = _context.SystemCommands
                    .Where(x => !x.IsDisabled)
                    .AsEnumerable()
                    .Select(x => new InlineKeyboardButton[] {
                        InlineKeyboardButton.WithCallbackData(x.Name, $"{SystemCommandPrefix}{x.Id}")
                    }).ToList();
            }

            inlineKeyboard = new(
                _context.SystemCommands
                .Where(x => !x.IsDisabled)
                .AsEnumerable()
                .Select(x => new InlineKeyboardButton[] {
                        InlineKeyboardButton.WithCallbackData(x.Name, $"{SystemCommandPrefix}{x.Id}")
                }).ToArray()
            );

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

    // Process Inline Keyboard callback data
    private async Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received inline keyboard callback from: {CallbackQueryId}", callbackQuery.Id);

        if (callbackQuery.Message is null || callbackQuery.From.IsBot)
            return;

        await _botClient.SendChatActionAsync(
               chatId: callbackQuery.Message.Chat.Id,
               chatAction: ChatAction.Typing,
               cancellationToken: cancellationToken);

        UserResponseModel check_user = CheckTelegramUser(callbackQuery.Message?.From);
        if (!check_user.IsSuccess || check_user.User?.IsDisabled == true)
            return;

        await _botClient.AnswerCallbackQueryAsync(
            callbackQueryId: callbackQuery.Id,
            text: $"Received {callbackQuery.Data}",
            cancellationToken: cancellationToken);

        string output = "< --- >";

        if (callbackQuery.Data?.StartsWith(SystemCommandPrefix, StringComparison.OrdinalIgnoreCase) == true)
        {
            string sys_com_id = callbackQuery.Data[SystemCommandPrefix.Length..];
            if (!Regex.IsMatch(sys_com_id, @"^\d+$") || int.TryParse(sys_com_id, out int sc_id))
                return;

            SystemCommandModelDB? sys_cmd_db;
            lock (ServerContext.DbLocker)
            {
                using ServerContext _db = new();
                sys_cmd_db = _db.SystemCommands.FirstOrDefault(x => x.Id == sc_id);
            }

            if (sys_cmd_db is null)
                return;

            Process p = new();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = sys_cmd_db.FileName;
            p.StartInfo.Arguments = sys_cmd_db.Arguments;
            p.Start();

            output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            if (output.Length > 4000)
                output = output[0..4000];
        }

        await _botClient.SendTextMessageAsync(
            chatId: callbackQuery.Message!.Chat.Id,
            text: $"Received {callbackQuery.Data}:\n\n{output}",
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

    private UserResponseModel CheckTelegramUser(User? from)
    {
        UserResponseModel res = new();
        if (from is null)
        {
            res.AddError("from is null. error {5513E9FE-75DB-4C9F-92A3-7537072AD3CD}");
            return res;
        }

        using ServerContext context = new();
        lock (ServerContext.DbLocker)
        {
            res.User = context.Users.FirstOrDefault(x => x.TelegramId == from.Id);
            if (res.User is null)
            {
                res.User = new()
                {
                    FirstName = from.FirstName,
                    LastName = from.LastName,
                    Name = from.Username ?? "",
                    TelegramId = from.Id,
                    AlarmSubscriber = false,
                    CommandsAllowed = false,
                    IsDisabled = true
                };
                context.Add(res.User);
                res.AddInfo("Объект создан");
            }
            else
            {
                res.User.FirstName = from.FirstName;
                res.User.LastName = from.LastName;
                res.User.Name = from.Username ?? "";
                res.User.LastUpdate = DateTime.Now;
                context.Update(res.User);
                res.AddInfo("Объект обновлён");
            }
            context.SaveChanges();
        }

        return res;
    }
}