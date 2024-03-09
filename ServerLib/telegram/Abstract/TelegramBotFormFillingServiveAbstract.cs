////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using ab.context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedLib;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ServerLib;

/// <summary>
/// Заполнение формы данными через TelegramBot
/// </summary>
public abstract class TelegramBotFormFillingServiveAbstract : ITelegramBotFormFillingServive
{
    /// <summary>
    /// Клиентский интерфейс для использования Telegram Bot API
    /// </summary>
    public required ITelegramBotClient BotClient { get; set; }

    /// <summary>
    /// Logger
    /// </summary>
    public required ILogger<TelegramBotFormFillingServiveAbstract> Logger { get; set; }

    /// <summary>
    /// ServerContext DB factory
    /// </summary>
    public required IDbContextFactory<ServerContext> DbFactory { get; set; }

    /// <inheritdoc/>
    public async Task<Message> FormFillingHandle(UserFormModelDb form, int message_id, TypeValueTelegramBotHandle type_handler, string? set_value, CancellationToken cancellation_token)
    {
        if (form.OwnerUser is null)
            throw new Exception("error {EBB2EFAE-61E7-4E53-86C7-99931FE22656}");

        using ServerContext db = DbFactory.CreateDbContext();
        FormMetadataModel form_metadata = FormFillingFlowsStatic.FormFillingFlows[nameof(MqttConfigModel)];
        if (form.Properties?.Count != 0)
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
                db.AddRange(form.Properties);
                db.SaveChanges();
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
            db.Update(next_prop);
            db.SaveChanges();
            await BotClient.DeleteMessageAsync(form.OwnerUser.ChatId, message_id, cancellationToken: cancellation_token);
            return await FormFillingHandle(form, message_id, TypeValueTelegramBotHandle.Message, "", cancellation_token);
        }

        form.Properties.ForEach(x => html_response += $"\n<{(string.IsNullOrEmpty(x.PropValue) ? "u" : "b")}>{x.Name}</{(string.IsNullOrEmpty(x.PropValue) ? "u" : "b")}>:<code>{x.PropValue}</code>");

        if (next_prop is null)
        {
            html_response += $"\n♦ ♦ ♦ ♦ ♦ ♦ ♦ ♦ ♦ ♦ ♦ ♦\n";
            html_response += "\nФорма заполнена - её можно применить/записать!\nЧто бы вернуться в начальное меню - /start";
        }
        else
        {
            html_response += $"\n** * ** * ** * ** * ** * **\n";
            html_response += $"{next_prop.Name} (введите значение):\n";
        }

        InlineKeyboardMarkup? kb = next_prop is null
                        ? new InlineKeyboardMarkup(new[] { new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData("Применить", nameof(MqttConfigModel)) } })
                        : null;
        try
        {
            return await BotClient.EditMessageTextAsync(form.OwnerUser.ChatId, form.OwnerUser.MessageId, html_response, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: kb, cancellationToken: cancellation_token);
        }
        catch (Exception ex)
        {
            Logger.LogError("error {BA749E66-C485-4EA0-A14D-C6ECF30F594C}", ex);
            Message msg = await BotClient.SendTextMessageAsync(form.OwnerUser.ChatId, html_response, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: kb, cancellationToken: cancellation_token);
            form.OwnerUser.MessageId = msg.MessageId;
            lock (ServerContext.DbLocker)
            {
                db.Update(form.OwnerUser);
                db.SaveChanges();
            }
            return msg;
        }
    }
}