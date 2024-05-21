////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using ab.context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServerLib;
using SharedLib;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Services;

/// <summary>
/// 
/// </summary>
public class UpdateHandler : IUpdateHandler
{
    readonly ITelegramBotFormFillingService _form_fill;
    readonly ITelegramBotHardwareViewServive _hardware_view;
    readonly IParametersStorageService _storage;
    readonly ILogger<UpdateHandler> _logger;
    readonly ITelegramBotClient _botClient;
    readonly IToolsService _tools;
    readonly IDbContextFactory<ServerContext> _db_factory;
    readonly INotifyService _notify;

    const string GetProcesses = "get-processes";
    const string SystemCommandPrefix = "sys:";
    const string RestartMQTT = "restart-mqtt";
    const string GetMQTT = "get-mqtt";

    /// <summary>
    /// 
    /// </summary>
    public UpdateHandler(ITelegramBotClient botClient, ILogger<UpdateHandler> logger, ITelegramBotFormFillingService form_fill, IParametersStorageService storage, IToolsService tools, ITelegramBotHardwareViewServive hardware_view, INotifyService notify, IDbContextFactory<ServerContext> db_factory)
    {
        _botClient = botClient;
        _logger = logger;
        _form_fill = form_fill;
        _storage = storage;
        _tools = tools;
        _hardware_view = hardware_view;
        _notify = notify;
        _db_factory = db_factory;
    }

    /// <summary>
    /// 
    /// </summary>
    public async Task HandleUpdateAsync(ITelegramBotClient _, Update update, CancellationToken cancellationToken)
    {
        var handler = update switch
        {
            { CallbackQuery: { } callbackQuery } => BotOnCallbackQueryReceived(callbackQuery, cancellationToken),
            { Message: { } message } => BotOnMessageReceived(message, cancellationToken),
            _ => UnknownUpdateHandlerAsync(update, cancellationToken)
            // UpdateType.Poll:
            // UpdateType.Unknown:
            // UpdateType.ChannelPost:
            // UpdateType.ShippingQuery:
            // UpdateType.PreCheckoutQuery:
            // UpdateType.EditedChannelPost:
            //{ EditedMessage: { } message } => BotOnMessageReceived(message, cancellationToken),
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

        TResponseModel<UserModelDB> check_user = CheckTelegramUser(message.From);
        _notify.CheckTelegramUser(check_user);

        using ServerContext context = _db_factory.CreateDbContext();

        if (!check_user.IsSuccess || check_user.Response is null || check_user.Response.IsDisabled)
            return;

        Task<Message> action = messageText.Split(' ')[0] switch
        {
            "/start" => GetStartMessage(check_user.Response, message, "Приветсвую!\n", cancellationToken),
            "/throw" => FailingHandler(_botClient, message, cancellationToken),
            _ => Usage(_botClient, check_user.Response, message, cancellationToken)
        };
        Message sentMessage = await action;
        _logger.LogInformation("The message was sent with id: {SentMessageId}", sentMessage.MessageId);

        async Task<Message> Usage(ITelegramBotClient botClient, UserModelDB user, Message message, CancellationToken cancellationToken)
        {
            string usage = "Бот-обормот";

            if (user.UserForm is not null)
                return await _form_fill.FormFillingHandle(user.UserForm, message.MessageId, TypeValueTelegramBotHandle.Message, message.Text, cancellationToken);

            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: usage,
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

        if (callbackQuery.Message is null || callbackQuery.From.IsBot || callbackQuery.Data is null)
            return;

        TResponseModel<UserModelDB> check_user = CheckTelegramUser(callbackQuery.From);
        _notify.CheckTelegramUser(check_user);

        using ServerContext context = _db_factory.CreateDbContext();

        if (!check_user.IsSuccess || check_user.Response is null || check_user.Response.IsDisabled == true)
            return;

        Message res_msg;
        if (check_user.Response.UserForm is not null)
        {
            if (check_user.Response.UserForm.Properties?.Any(x => string.IsNullOrEmpty(x.PropValue)) == true)
            {
                res_msg = await _form_fill.FormFillingHandle(check_user.Response.UserForm, callbackQuery.Message.MessageId, TypeValueTelegramBotHandle.CallbackQuery, callbackQuery.Data, cancellationToken);
            }
            else if (check_user.Response.UserForm.Properties?.Any() == true)
            {
                switch (check_user.Response.UserForm.FormMapCode)
                {
                    case nameof(MqttConfigModel):
                        UserFormPropertyModelDb username_prop = check_user.Response.UserForm.Properties.First(x => x.Code.Equals(nameof(MqttConfigModel.Username)));
                        UserFormPropertyModelDb password_prop = check_user.Response.UserForm.Properties.First(x => x.Code.Equals(nameof(MqttConfigModel.Password)));
                        UserFormPropertyModelDb server_prop = check_user.Response.UserForm.Properties.First(x => x.Code.Equals(nameof(MqttConfigModel.Server)));
                        UserFormPropertyModelDb port_prop = check_user.Response.UserForm.Properties.First(x => x.Code.Equals(nameof(MqttConfigModel.Port)));
                        UserFormPropertyModelDb secret_prop = check_user.Response.UserForm.Properties.First(x => x.Code.Equals(nameof(MqttConfigModel.Secret)));
                        UserFormPropertyModelDb topics_prefix_prop = check_user.Response.UserForm.Properties.First(x => x.Code.Equals(nameof(MqttConfigModel.PrefixMqtt)));

                        if (!int.TryParse(port_prop?.PropValue, out int port_int))
                        {
                            callbackQuery.Message.Text = "/start";
                            res_msg = await GetStartMessage(check_user.Response, callbackQuery.Message, "<u>Ошибка значения порта. Парамтры не применились!</u>\n", cancellationToken);
                        }
                        else
                        {
                            TResponseModel<MqttConfigModel> conf_db = await _storage.GetMqttConfig(cancellationToken);
                            if (conf_db.Response is null)
                            {
                                callbackQuery.Message.Text = "/start";
                                res_msg = await GetStartMessage(check_user.Response, callbackQuery.Message, "<u>conf_db.Conf is null. error {F49ED342-4555-4B48-845F-98F2CF412F0D}!</u>\n", cancellationToken);
                            }
                            else
                            {
                                conf_db.Response.Username = username_prop.PropValue ?? "";
                                conf_db.Response.Password = password_prop.PropValue ?? "";
                                conf_db.Response.Server = server_prop.PropValue ?? "";
                                conf_db.Response.Port = port_int;
                                conf_db.Response.Secret = secret_prop.PropValue ?? "";
                                conf_db.Response.PrefixMqtt = topics_prefix_prop.PropValue;

                                await _storage.SaveMqttConfig(conf_db.Response, cancellationToken);
                                callbackQuery.Message.Text = "/start";
                                res_msg = await GetStartMessage(check_user.Response, callbackQuery.Message, "<u>Параметры сохранены! Перезапустите MQTT что бы параметры применились...</u>\n", cancellationToken);
                            }
                        }

                        break;
                    default:

                        break;
                }
            }

            return;
        }

        string output = callbackQuery.Message.Text ?? "[---]", pre_out;
        using ServerContext _db = _db_factory.CreateDbContext();
        UserFormModelDb form;

        if (callbackQuery.Data.Equals("/start"))
        {
            await _botClient.SendChatActionAsync(
                   chatId: callbackQuery.Message.Chat.Id,
                   chatAction: ChatAction.Typing,
                   cancellationToken: cancellationToken);

            await GetStartMessage(check_user.Response, callbackQuery.Message, null, cancellationToken);
            return;
        }

        else if (callbackQuery.Data.Equals(GetMQTT) && check_user.Response.AllowChangeMqttConfig)
        {
            await _botClient.SendChatActionAsync(
                   chatId: callbackQuery.Message.Chat.Id,
                   chatAction: ChatAction.Typing,
                   cancellationToken: cancellationToken);

            string message_report = "";
            TResponseModel<MqttConfigModel> conf_db = await _storage.GetMqttConfig(cancellationToken);
            if (!conf_db.IsSuccess)
            {
                message_report += $"Ошибка чтения конфигурации: {conf_db.Message}";
            }
            else if (conf_db.Response is null)
            {
                message_report += "conf_db.Conf is null. error {47C6FB81-3E66-4754-A1D2-50C15D87B4FF}";
            }
            else
            {
                Type myType = conf_db.Response.GetType();
                IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());

                foreach (PropertyInfo prop in props)
                {
                    object? propValue = prop.GetValue(conf_db.Response, null);
                    message_report += $"\n<b>{prop.Name}</b>: <code>{propValue}</code>";
                }
            }

            TResponseModel<bool> ststus_mqtt = await _tools.StatusMqtt();
            message_report += $"\n-- || -- - -- - -- - || - -- - -- - -- || --\n<u>Status MQTT</u>: {ststus_mqtt.Message}";

            _ = await _botClient.SendTextMessageAsync(check_user.Response.ChatId, message_report, parseMode: ParseMode.Html, cancellationToken: cancellationToken);
            return;
        }
        else if (callbackQuery.Data.Equals(nameof(UserFormModelDb)) && check_user.Response.AllowChangeMqttConfig)
        {
            lock (ServerContext.DbLocker)
            {
                if (check_user.Response.UserForm is not null && _db.UsersForms.Any(x => x.Id == check_user.Response.UserForm.Id))
                {
                    _db.Remove(check_user.Response.UserForm);
                    _db.SaveChanges(true);
                }
                FormMetadataModel form_metadata = FormFillingFlowsStatic.FormFillingFlows[nameof(MqttConfigModel)];
                form = new()
                {
                    OwnerUserId = check_user.Response.Id,
                    FormMapCode = nameof(MqttConfigModel)
                };

                _db.Add(form);
                _db.SaveChanges(true);
                form.Properties = form_metadata.FormProperties.Select(x => new UserFormPropertyModelDb() { Code = x.Code, Name = x.Title, OwnerFormId = form.Id }).ToList();
                _db.AddRange(form.Properties);
                _db.SaveChanges(true);
                check_user.Response.UserForm = _db.UsersForms.Include(x => x.OwnerUser).FirstOrDefault(x => x.OwnerUserId == check_user.Response.Id);
            }

            form.Properties.ForEach(x => x.OwnerForm = form);
            form.OwnerUser = check_user.Response;

            if (check_user.Response.UserForm is null)
            {
                _logger.LogError($"check_user.User.UserForm is null. error C0AB0CE4-5442-4617-A324-A8E3A4D0E414");
                return;
            }

            res_msg = await _form_fill.FormFillingHandle(check_user.Response.UserForm, callbackQuery.Message.MessageId, TypeValueTelegramBotHandle.CallbackQuery, callbackQuery.Data, cancellationToken);
            return;
        }

        else if (callbackQuery.Data.Equals(RestartMQTT) && check_user.Response.AllowSystemCommands)
        {
            await _botClient.SendChatActionAsync(
                   chatId: callbackQuery.Message.Chat.Id,
                   chatAction: ChatAction.Typing,
                   cancellationToken: cancellationToken);

            string message_report = "Перезапуск/запуск службы MQTT.\n";
            ResponseBaseModel start_mqtt = await _tools.StartMqtt(cancellationToken);
            message_report += $"\n{start_mqtt.Message}";
            _ = await _botClient.SendTextMessageAsync(check_user.Response.ChatId, message_report, parseMode: ParseMode.Html, cancellationToken: cancellationToken);
            return;
        }
        else if (callbackQuery.Data.Equals(GetProcesses) && check_user.Response.AllowSystemCommands)
        {
            await _botClient.SendChatActionAsync(
                   chatId: callbackQuery.Message.Chat.Id,
                   chatAction: ChatAction.Typing,
                   cancellationToken: cancellationToken);

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
            await _botClient.SendTextMessageAsync(callbackQuery.Message!.Chat.Id, output.Trim(),
                            parseMode: ParseMode.Html,
                            cancellationToken: cancellationToken);
            await GetStartMessage(check_user.Response, null, cancellationToken: cancellationToken);
            return;
        }
        else if (callbackQuery.Data.StartsWith(SystemCommandPrefix) && check_user.Response.AllowSystemCommands)
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
        else if (callbackQuery.Data.StartsWith(GlobalStatic.Routes.AbPrefix) && check_user.Response.CommandsAllowed)
        {
            callbackQuery.Data = callbackQuery.Data[GlobalStatic.Routes.AbPrefix.Length..];
            Match match = Regex.Match(callbackQuery.Data, @"^(?<hw_id>\d+)");
            callbackQuery.Data = callbackQuery.Data[(match.Groups["hw_id"].Value.Length)..];
            if (callbackQuery.Data.StartsWith(':'))
                callbackQuery.Data = callbackQuery.Data[1..];
            res_msg = await _hardware_view.HardwareViewMainHandle(check_user.Response.ChatId, check_user.Response.MessageId, callbackQuery.Data, int.Parse(match.Groups["hw_id"].Value), cancellationToken);
            return;
        }
        else if (callbackQuery.Data.StartsWith($"{GlobalStatic.Routes.Port}:") && check_user.Response.CommandsAllowed)
        {
            callbackQuery.Data = callbackQuery.Data[(GlobalStatic.Routes.Port.Length + 1)..];
            Match match = Regex.Match(callbackQuery.Data, @"^(?<pt_id>\d+)");
            callbackQuery.Data = callbackQuery.Data[match.Groups["pt_id"].Value.Length..];
            if (callbackQuery.Data.StartsWith(":"))
                callbackQuery.Data = callbackQuery.Data[1..];
            if (callbackQuery.Data.StartsWith("&"))
                callbackQuery.Data = callbackQuery.Data[1..];

            string cmd_set = $"cmd={match.Groups["pt_id"].Value}:";
            if (callbackQuery.Data.StartsWith(cmd_set))
                callbackQuery.Data = callbackQuery.Data[cmd_set.Length..];

            res_msg = await _hardware_view.HardwarePortViewHandle(check_user.Response.ChatId, check_user.Response.MessageId, callbackQuery.Data, int.Parse(match.Groups["pt_id"].Value), cancellationToken);

            if (res_msg.MessageId != check_user.Response.MessageId)
            {
                check_user.Response.MessageId = res_msg.MessageId;
                lock (ServerContext.DbLocker)
                {
                    _db.Update(check_user.Response);
                    _db.SaveChanges(true);
                }
            }

            return;
        }

        try
        {
            await _botClient.EditMessageTextAsync(
                chatId: check_user.Response.ChatId,
                messageId: check_user.Response.MessageId,
                text: $"Received {callbackQuery.Data}:\n\n{output}",
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError("error {B55C5A58-3D2A-4782-92FA-CC16D2FBFEBB}", ex);

            Message msg = await _botClient.SendTextMessageAsync(
                chatId: check_user.Response.ChatId,
                text: $"Received {callbackQuery.Data}:\n\n{output}",
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken);

            lock (ServerContext.DbLocker)
            {
                check_user.Response.ChatId = msg.Chat.Id;
                check_user.Response.MessageId = msg.MessageId;
                _db.Update(check_user.Response);
                _db.SaveChanges();
            }
        }
    }

    async Task<Message> GetStartMessage(UserModelDB User, Message? message_request, string? caption = null, CancellationToken cancellationToken = default)
    {
        string usage = caption ?? "Бот-обормот";

        using ServerContext _context = _db_factory.CreateDbContext();
        lock (ServerContext.DbLocker)
        {
            User.UserForm = _context.UsersForms.Include(x => x.Properties).FirstOrDefault(x => x.OwnerUserId == User.Id);
            if (User.UserForm is not null)
            {
                _context.Remove(User.UserForm);
                User.UserForm = null;
                _context.SaveChanges(true);
            }
        }

        List<InlineKeyboardButton[]> kb_rows = [];
        if (User.AllowSystemCommands)
        {
            lock (ServerContext.DbLocker)
            {
                kb_rows = _context.SystemCommands
                    .Where(x => !x.IsDisabled)
                    .AsEnumerable()
                    .Select(x => new InlineKeyboardButton[] {
                        InlineKeyboardButton.WithCallbackData($"-- {x.Name} --", $"{SystemCommandPrefix}{x.Id}")
                    }).ToList();
            }

            kb_rows.Insert(0, [InlineKeyboardButton.WithCallbackData("Процессы", GetProcesses)]);
        }
        if (User.AllowChangeMqttConfig)
        {
            kb_rows.Add([InlineKeyboardButton.WithCallbackData("MQTT - настроить", nameof(UserFormModelDb))]);
            kb_rows.Add([InlineKeyboardButton.WithCallbackData("MQTT - состояние", GetMQTT)]);
            kb_rows.Add([InlineKeyboardButton.WithCallbackData("MQTT - перезапуск", RestartMQTT)]);
        }
        if (User.CommandsAllowed)
        {
            HardwareModelDB[] hardwires;
            lock (ServerContext.DbLocker)
            {
                hardwires = [.. _context.Hardwires];
            }
            foreach (HardwareModelDB hw in hardwires)
            {
                kb_rows.Add([InlineKeyboardButton.WithCallbackData($"•• {(string.IsNullOrWhiteSpace(hw.Name) ? hw.Address : hw.Name)} ••", $"{GlobalStatic.Routes.AbPrefix}{hw.Id}")]);
            }
        }

        long chat_id_old = User.ChatId;
        int message_id_old = User.MessageId;
        User.MessageId = default;
        lock (ServerContext.DbLocker)
        {
            _context.Update(User);
            _context.SaveChanges(true);
        }

        Message msg_res = await _botClient.SendTextMessageAsync(message_request?.Chat.Id ?? chat_id_old, usage,
        parseMode: ParseMode.Html,
        replyMarkup: kb_rows.Count != 0 ? new InlineKeyboardMarkup(kb_rows) : new ReplyKeyboardRemove(),
        cancellationToken: cancellationToken);
        //
        User.ChatId = msg_res.Chat.Id;
        User.MessageId = msg_res.MessageId;
        lock (ServerContext.DbLocker)
        {
            _context.Update(User);
            _context.SaveChanges(true);
        }

        if (message_id_old != default && chat_id_old != default)
        {
            try
            {
                await _botClient.DeleteMessageAsync(chat_id_old, message_id_old, cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("error {B8F28B15-5E87-444C-8718-9FA44724CF23}", ex);
            }
        }
        if (message_request is not null)
            await _botClient.DeleteMessageAsync(message_request.Chat.Id, message_request.MessageId, cancellationToken: cancellationToken);
        return msg_res;
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

    private TResponseModel<UserModelDB> CheckTelegramUser(User? from)
    {
        TResponseModel<UserModelDB> res = new();
        if (from is null)
        {
            res.AddError("from is null. error {5513E9FE-75DB-4C9F-92A3-7537072AD3CD}");
            return res;
        }

        using ServerContext context = _db_factory.CreateDbContext();
        lock (ServerContext.DbLocker)
        {
            res.Response = context.Users
                .Include(x => x.UserForm)
                .ThenInclude(x => x!.Properties)
                .FirstOrDefault(x => x.TelegramId == from.Id);

            if (res.Response is null)
            {
                res.Response = new()
                {
                    FirstName = from.FirstName,
                    LastName = from.LastName,
                    Name = from.Username ?? "",
                    TelegramId = from.Id,
                    AlarmSubscriber = false,
                    CommandsAllowed = false,
                    AllowChangeMqttConfig = false,
                    AllowSystemCommands = false,
                    IsDisabled = true
                };
                context.Add(res.Response);
                res.AddInfo("Пользователь Telegram: создан");
            }
            else
            {
                res.Response.FirstName = from.FirstName;
                res.Response.LastName = from.LastName;
                res.Response.Name = from.Username ?? "";
                res.Response.LastUpdate = DateTime.Now;
                context.Update(res.Response);
                res.AddInfo("Данные Telegram пользователя: обновлены");
            }

            context.SaveChanges(true);

            res.Response = context.Users
                .Include(x => x.UserForm)
                .ThenInclude(x => x!.Properties)
                .FirstOrDefault(x => x.TelegramId == from.Id);
        }

        return res;
    }
}