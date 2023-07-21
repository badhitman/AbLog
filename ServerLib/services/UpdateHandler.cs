using Telegram.Bot.Types.ReplyMarkups;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using System.Diagnostics;
using Telegram.Bot.Types;
using ab.context;
using SharedLib;
using ServerLib;

namespace Telegram.Bot.Services;

/// <summary>
/// 
/// </summary>
public class UpdateHandler : IUpdateHandler
{
    readonly ITelegramBotFormFillingServive _form_fill;
    readonly ILogger<UpdateHandler> _logger;
    readonly ITelegramBotClient _botClient;

    const string SystemCommandPrefix = "sys:";
    const string GetProcesses = "get-processes";
    //const string SetupMQTT = "mqtt-setup";

    /// <summary>
    /// 
    /// </summary>
    public UpdateHandler(ITelegramBotClient botClient, ILogger<UpdateHandler> logger, ITelegramBotFormFillingServive form_fill)
    {
        _botClient = botClient;
        _logger = logger;
        _form_fill = form_fill;
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
            "/start" => GetStartMessage(check_user.User, message, cancellationToken),
            "/throw" => FailingHandler(_botClient, message, cancellationToken),
            _ => Usage(_botClient, check_user.User, message, cancellationToken)
        };
        Message sentMessage = await action;
        _logger.LogInformation("The message was sent with id: {SentMessageId}", sentMessage.MessageId);

        static async Task<Message> Usage(ITelegramBotClient botClient, UserModelDB User, Message message, CancellationToken cancellationToken)
        {
            string usage = "Бот-обормот";

            //using ServerContext _context = new();

            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: usage,
                cancellationToken: cancellationToken);
        }

        async Task<Message> GetStartMessage(UserModelDB User, Message message, CancellationToken cancellationToken)
        {
            if (message.Text?.Equals("/start") == true)
                try
                {
                    await _botClient.DeleteMessageAsync(message.Chat.Id, message.MessageId, cancellationToken: cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError("error {C1BAB78B-5075-4A75-BA55-49DB319A0AD2}", ex);
                }

            string usage = "Бот-обормот";

            using ServerContext _context = new();
            lock (ServerContext.DbLocker)
            {
                UserFormModelDb? actual_form = _context.UsersForms.Include(x => x.Properties).FirstOrDefault(x => x.OwnerUserId == User.Id);
                if (actual_form is not null)
                {
                    _context.Remove(actual_form);
                    _context.SaveChanges();
                }
            }

            if (User.MessageId != default && User.ChatId != default)
            {
                long chat_id = User.ChatId;
                int message_id = User.MessageId;
                User.ChatId = default;
                User.MessageId = default;
                lock (ServerContext.DbLocker)
                {
                    _context.Update(User);
                    _context.SaveChanges();
                }
                try
                {
                    await _botClient.DeleteMessageAsync(chat_id, message_id, cancellationToken: cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError("error {B8F28B15-5E87-444C-8718-9FA44724CF23}", ex);
                }
            }

            List<InlineKeyboardButton[]> kb_rows = new();

            if (User.AllowSystemCommands)
            {
                lock (ServerContext.DbLocker)
                {
                    kb_rows = _context.SystemCommands
                        .Where(x => !x.IsDisabled)
                        .AsEnumerable()
                        .Select(x => new InlineKeyboardButton[] {
                        InlineKeyboardButton.WithCallbackData($"> {x.Name}", $"{SystemCommandPrefix}{x.Id}")
                        }).ToList();
                }

                kb_rows.Insert(0, new[] { InlineKeyboardButton.WithCallbackData("Процессы", $"{GetProcesses}") });
            }

            if (User.AllowChangeMqttConfig)
            {
                kb_rows.Add(new[] { InlineKeyboardButton.WithCallbackData("Настроить MQTT", nameof(MqttConfigModel)) });
            }

            Message msg_res = await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: usage,
            replyMarkup: kb_rows.Any() ? new InlineKeyboardMarkup(kb_rows) : new ReplyKeyboardRemove(),
            cancellationToken: cancellationToken);

            User.ChatId = msg_res.Chat.Id;
            User.MessageId = msg_res.MessageId;
            lock (ServerContext.DbLocker)
            {
                _context.Update(User);
                _context.SaveChanges();
            }

            return msg_res;
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

        if (callbackQuery.Message is null || callbackQuery.From.IsBot || callbackQuery.Data is null)
            return;

        await _botClient.SendChatActionAsync(
               chatId: callbackQuery.Message.Chat.Id,
               chatAction: ChatAction.Typing,
               cancellationToken: cancellationToken);

        UserResponseModel check_user = CheckTelegramUser(callbackQuery.From);
        if (!check_user.IsSuccess || check_user.User is null || check_user.User.IsDisabled == true)
            return;

        if (check_user.User.UserForm is not null)
        {
            await _form_fill.FormFillingHandle(check_user.User.UserForm, null, cancellationToken);
            return;
        }

        await _botClient.AnswerCallbackQueryAsync(
            callbackQueryId: callbackQuery.Id,
            text: $"Received {callbackQuery.Data}",
            cancellationToken: cancellationToken);

        string output = "< --- >", pre_out;
        using ServerContext _db = new();
        UserFormModelDb form;
        if (callbackQuery.Data.Equals(nameof(MqttConfigModel)) == true)
        {
            if (!check_user.User.AllowChangeMqttConfig)
                return;

            lock (ServerContext.DbLocker)
            {
                if (check_user.User.UserForm is not null && _db.UsersForms.Any(x => x.Id == check_user.User.UserForm.Id))
                {
                    _db.Remove(check_user.User.UserForm);
                    _db.SaveChanges();
                }
                FormMetadataModel form_metadata = FormFillingFlowsStatic.FormFillingFlows[nameof(MqttConfigModel)];
                form = new()
                {
                    OwnerUserId = check_user.User.Id,
                    FormMapCode = nameof(MqttConfigModel)
                };

                _db.Add(form);
                _db.SaveChanges();
                form.Properties = form_metadata.MqttConfigFormPropertyes.Select(x => new UserFormPropertyModelDb() { Code = x.Code, Name = x.Title, OwnerFormId = form.Id }).ToList();
                _db.AddRange(form.Properties);
                _db.SaveChanges();
                check_user.User.UserForm = _db.UsersForms.Include(x => x.OwnerUser).FirstOrDefault(x => x.OwnerUserId == check_user.User.Id);
            }

            form.Properties.ForEach(x => x.OwnerForm = form);
            form.OwnerUser = check_user.User;

            if (check_user.User.UserForm is null)
            {
                _logger.LogError($"check_user.User.UserForm is null. error C0AB0CE4-5442-4617-A324-A8E3A4D0E414");
                return;
            }

            await _form_fill.FormFillingHandle(check_user.User.UserForm, null, cancellationToken);
            return;
        }
        else if (callbackQuery.Data.Equals(GetProcesses) == true && check_user.User.AllowSystemCommands)
        {
            Process[] localAll = Process.GetProcesses().OrderBy(x => x.ProcessName).ToArray();
            output = "Все процессы:\n";
            long total_mem_used = 0;
            foreach (Process process in localAll)
            {
                total_mem_used += process.WorkingSet64;
                pre_out = $"\n~ <b>{process.Id}</b> <code>{process.ProcessName}</code> <u>{GlobalStatic.SizeDataAsString(process.WorkingSet64)}</u>";
                if (output.Length + pre_out.Length <= 4000)
                {
                    output += $"{pre_out}";
                }
                else
                {
                    await _botClient.SendTextMessageAsync(
                            chatId: callbackQuery.Message!.Chat.Id,
                            text: $"{output.Trim()}\n\n... продолжение следует",
                            parseMode: ParseMode.Html,
                            cancellationToken: cancellationToken);

                    output = $"продолжение:...\n{pre_out}";
                }
            }

            output += $"\n------------\ntotal mem.: <b>{GlobalStatic.SizeDataAsString(total_mem_used)}</b>";
        }
        else if (callbackQuery.Data.StartsWith(SystemCommandPrefix, StringComparison.OrdinalIgnoreCase) == true && check_user.User.AllowSystemCommands)
        {
            string sys_com_id = callbackQuery.Data[SystemCommandPrefix.Length..];

            if (!Regex.IsMatch(sys_com_id, @"^\d+$") || int.TryParse(sys_com_id, out int sc_id))
                return;

            SystemCommandModelDB? sys_cmd_db;
            lock (ServerContext.DbLocker)
            {
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

            output = await p.StandardOutput.ReadToEndAsync();
            await p.WaitForExitAsync(cancellationToken);
            if (output.Length > 4000)
                output = output[0..4000];
        }

        try
        {
            await _botClient.EditMessageTextAsync(
                chatId: check_user.User.ChatId,
                messageId: check_user.User.MessageId,
                text: $"Received {callbackQuery.Data}:\n\n{output}",
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError("error {B55C5A58-3D2A-4782-92FA-CC16D2FBFEBB}", ex);

            Message msg = await _botClient.SendTextMessageAsync(
                chatId: check_user.User.ChatId,
                text: $"Received {callbackQuery.Data}:\n\n{output}",
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken);

            lock (ServerContext.DbLocker)
            {
                check_user.User.ChatId = msg.Chat.Id;
                check_user.User.MessageId = msg.MessageId;
                _db.Update(check_user.User);
                _db.SaveChanges();
            }
        }
    }

    private Task UnknownUpdateHandlerAsync(Update update, CancellationToken cancellationToken)
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
            res.User = context.Users.Include(x => x.UserForm).FirstOrDefault(x => x.TelegramId == from.Id);
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
                    AllowChangeMqttConfig = false,
                    AllowSystemCommands = false,
                    LastUpdate = DateTime.Now,
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