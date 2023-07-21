using Telegram.Bot.Types.ReplyMarkups;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using ab.context;
using SharedLib;

namespace ServerLib;

/// <summary>
/// 
/// </summary>
public class TelegramBotFormFillingServive : ITelegramBotFormFillingServive
{
    readonly ILogger<TelegramBotFormFillingServive> _logger;
    readonly ITelegramBotClient _botClient;

    /// <summary>
    /// 
    /// </summary>
    public TelegramBotFormFillingServive(ITelegramBotClient botClient, ILogger<TelegramBotFormFillingServive> logger)
    {
        _botClient = botClient;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task FormFillingHandle(UserFormModelDb form, string? set_value, CancellationToken cancellation_token)
    {
        if (form.OwnerUser is null)
            return;

        using ServerContext _db = new();
        FormMetadataModel form_metadata = FormFillingFlowsStatic.FormFillingFlows[nameof(MqttConfigModel)];
        if (form.Properties?.Any() != true)
        {
            form.Properties = form_metadata
                .MqttConfigFormPropertyes
                .Select(x => new UserFormPropertyModelDb()
                {
                    Name = x.Title,
                    Code = x.Code,
                    OwnerFormId = form.OwnerUser.Id
                }).ToList();

            lock (ServerContext.DbLocker)
            {
                _db.AddRange(form.Properties);
                _db.SaveChanges();
            }

            form.Properties.ForEach(x => x.OwnerForm = form);
        }
        UserFormPropertyModelDb? next_prop = form.Properties.FirstOrDefault(x => string.IsNullOrWhiteSpace(x.PropValue));

        string html_response = $"{form_metadata.Title}\n** * ** заполнение формы ** * **\n";
        form.Properties.ForEach(x => html_response += $"\n{x.Name}:{x.PropValue}");
        html_response += $"\n** * ** * ** * ** * ** * **";
        List<KeyboardButton> keyboardButtons = new();

        /*
         ReplyKeyboardMarkup replyKeyboardMarkup = new(
                new[]
                {
                        new KeyboardButton[] { "1.1", "1.2" },
                        new KeyboardButton[] { "2.1", "2.2" },
                })
            {
                ResizeKeyboard = true
            };
         */

        /*
         List<InlineKeyboardButton[]> kb_rows = new();
         replyMarkup: kb_rows.Any() ? new InlineKeyboardMarkup(kb_rows) : new ReplyKeyboardRemove(),
         */

        //IReplyMarkup kb;

        if (next_prop is null)
        {

        }
        else
        {

        }

        try
        {
            await _botClient.EditMessageTextAsync(form.OwnerUser.ChatId, form.OwnerUser.MessageId, html_response, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, cancellationToken: cancellation_token);
        }
        catch (Exception ex)
        {
            _logger.LogError("error {BA749E66-C485-4EA0-A14D-C6ECF30F594C}", ex);
            Telegram.Bot.Types.Message msg = await _botClient.SendTextMessageAsync(form.OwnerUser.ChatId, html_response, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, cancellationToken: cancellation_token);
            form.OwnerUser.MessageId = msg.MessageId;
            lock (ServerContext.DbLocker)
            {
                _db.Update(form.OwnerUser);
                _db.SaveChanges();
            }
        }
    }
}