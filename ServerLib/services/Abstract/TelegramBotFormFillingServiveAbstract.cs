using Telegram.Bot.Types.ReplyMarkups;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot;
using ab.context;
using SharedLib;

namespace ServerLib;

/// <summary>
/// 
/// </summary>
public abstract class TelegramBotFormFillingServiveAbstract : ITelegramBotFormFillingServive
{
    /// <summary>
    /// 
    /// </summary>
    public ITelegramBotClient _botClient { get; set; } = default!;

    /// <summary>
    /// 
    /// </summary>
    public ILogger<TelegramBotFormFillingServiveAbstract> _logger { get; set; } = default!;

    /// <inheritdoc/>
    public async Task<Message> FormFillingHandle(UserFormModelDb form, int message_id, TypeValueTelegramBotHandle type_handler, string? set_value, CancellationToken cancellation_token)
    {
        if (form.OwnerUser is null)
            throw new Exception("error {EBB2EFAE-61E7-4E53-86C7-99931FE22656}");

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

        string html_response = $"{form_metadata.Title}\n** * ** заполнение формы ** * **";
        UserFormPropertyModelDb? next_prop = form.Properties
            .OrderBy(x => x.Id)
            .FirstOrDefault(x => string.IsNullOrEmpty(x.PropValue));

        if (next_prop is not null && type_handler == TypeValueTelegramBotHandle.Message && !string.IsNullOrEmpty(set_value))
        {
            next_prop.PropValue = set_value;
            _db.Update(next_prop);
            _db.SaveChanges();
            await _botClient.DeleteMessageAsync(form.OwnerUser.ChatId, message_id, cancellationToken: cancellation_token);
            return await FormFillingHandle(form, message_id, TypeValueTelegramBotHandle.Message, "", cancellation_token);
        }

        form.Properties.ForEach(x => html_response += $"\n<{(string.IsNullOrEmpty(x.PropValue) ? "u" : "b")}>{x.Name}</{(string.IsNullOrEmpty(x.PropValue) ? "u" : "b")}>:<code>{x.PropValue}</code>");
        

        //IReplyMarkup kb = new ReplyKeyboardRemove();

        if (next_prop is null)
        {
            html_response += $"\n♦ ♦ ♦ ♦ ♦ ♦ ♦ ♦ ♦ ♦ ♦ ♦\n";
            html_response += "\nФорма заполнена - её можно применить/записать!\nЧто бы вернуться в начальное меню - /start";
            //    kb = new InlineKeyboardMarkup(new[] { new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData(ApplyButtonTitle) }, new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData(ReInitButtonTitle) } });
        }
        else
        {//♦ ♦ ♦ ♦ ♦ ♦ ♦ ♦ ♦
            html_response += $"\n** * ** * ** * ** * ** * **\n";
            html_response += $"{next_prop.Name} (введите значение):\n";
            //    kb = new ReplyKeyboardRemove();
        }

        //List<KeyboardButton> keyboardButtons = new();


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
        InlineKeyboardMarkup? kb = next_prop is null
                        ? new InlineKeyboardMarkup(new[] { new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData("Применить", nameof(MqttConfigModel)) } })
                        : null;
        try
        {

            return await _botClient.EditMessageTextAsync(form.OwnerUser.ChatId, form.OwnerUser.MessageId, html_response, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: kb, cancellationToken: cancellation_token);
        }
        catch (Exception ex)
        {
            _logger.LogError("error {BA749E66-C485-4EA0-A14D-C6ECF30F594C}", ex);
            Message msg = await _botClient.SendTextMessageAsync(form.OwnerUser.ChatId, html_response, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: kb, cancellationToken: cancellation_token);
            form.OwnerUser.MessageId = msg.MessageId;
            lock (ServerContext.DbLocker)
            {
                _db.Update(form.OwnerUser);
                _db.SaveChanges();
            }
            return msg;
        }
    }
}