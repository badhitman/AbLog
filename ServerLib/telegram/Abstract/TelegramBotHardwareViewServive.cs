////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Telegram.Bot.Types.ReplyMarkups;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot;
using ab.context;
using SharedLib;
using Telegram.Bot.Types.Enums;
using System.Text.RegularExpressions;

namespace ServerLib;

/// <summary>
/// 
/// </summary>
public class TelegramBotHardwareViewServive : ITelegramBotHardwareViewServive
{
    readonly ITelegramBotClient _botClient;
    readonly ILogger<TelegramBotHardwareViewServive> _logger;
    readonly IHardwaresService _hw;

    /// <summary>
    /// 
    /// </summary>
    public TelegramBotHardwareViewServive(ITelegramBotClient botClient, ILogger<TelegramBotHardwareViewServive> logger, IHardwaresService hw)
    {
        _botClient = botClient;
        _logger = logger;
        _hw = hw;
    }

    /// <inheritdoc/>
    public async Task<Message> HardwareViewMainHandle(long chat_id, int message_id, string? set_value, int hardware_id, CancellationToken cancellation_token = default)
    {
        HardwareModelDB? hw_db;
        using ServerContext context = new();
        lock (ServerContext.DbLocker)
        {
            hw_db = context.Hardwares.Include(x => x.Ports).FirstOrDefault(x => x.Id == hardware_id);
        }
        string output = "";
        if (hw_db is null)
        {
            output = "hw_db is null. error {65D334FE-8131-405C-81C3-C7EA72517DC4}";
            return await _botClient.EditMessageTextAsync(chat_id, message_id, output, cancellationToken: cancellation_token);
        }

        if (!string.IsNullOrWhiteSpace(hw_db.Name))
            output += $"[<b>{nameof(hw_db.Name)}</b>:<code>{hw_db.Name}</code>]\n";

        output += $"[<b>{nameof(hw_db.Address)}</b>:<code>{hw_db.Address}</code>]";

        if (hw_db.Ports?.Any() != true)
        {
            output += "hw_db.Ports?.Any() != true. error {DF48A07D-098E-4B2C-A5B0-7B6B162566EF}";
            return await _botClient.EditMessageTextAsync(chat_id, message_id, output, parseMode: ParseMode.Html, cancellationToken: cancellation_token);
        }

        List<InlineKeyboardButton[]> kb_rows = new();

        if (string.IsNullOrWhiteSpace(set_value))
        {
            foreach (PortModelDB[] chunks_ports in hw_db.Ports.OrderBy(x => x.PortNum).Chunk(3))
            {
                kb_rows.Add(chunks_ports.Select(x => InlineKeyboardButton.WithCallbackData($"→ {(string.IsNullOrWhiteSpace(x.Name) ? $"№{x.PortNum}" : $"'{x.Name}'")}", $"{GlobalStatic.Routes.Port}:{x.Id}")).ToArray());
            }
        }
        else if (set_value.StartsWith($"{GlobalStatic.Routes.Port}:"))
        {
            set_value = set_value[(GlobalStatic.Routes.Port.Length + 1)..];
            Match match = Regex.Match(set_value, @"^(?<port_id>\d+)");
            set_value = set_value[match.Groups["port_id"].Value.Length..];
            return await HardwarePortViewHandle(chat_id, message_id, set_value, int.Parse(match.Groups["port_id"].Value), cancellation_token);
        }

        return await _botClient.EditMessageTextAsync(chat_id, message_id, output, replyMarkup: new InlineKeyboardMarkup(kb_rows), parseMode: ParseMode.Html, cancellationToken: cancellation_token);
    }

    /// <inheritdoc/>
    public async Task<Message> HardwarePortViewHandle(long chat_id, int message_id, string? set_value, int port_id, CancellationToken cancellation_token = default)
    {
        PortModelDB? port_db;
        using ServerContext context = new();
        lock (ServerContext.DbLocker)
        {
            port_db = context.Ports.Include(x => x.Hardware).FirstOrDefault(x => x.Id == port_id);
        }

        string output = "";
        if (port_db?.Hardware is null)
        {
            output = "port_db?.Hardware is null. error {9D8C8A6D-D1EC-4D45-A45D-9C5A56C494A3}";
            return await _botClient.EditMessageTextAsync(chat_id, message_id, output, cancellationToken: cancellation_token);
        }

        if (!string.IsNullOrWhiteSpace(port_db.Hardware.Name))
            output += $"[<b>{nameof(port_db.Hardware.Name)}</b>: <code>{port_db.Hardware.Name}</code>]\n";

        output += $"[<b>{nameof(port_db.Hardware.Address)}</b>: <code>{port_db.Hardware?.Address}</code>]\n";

        if (!string.IsNullOrWhiteSpace(port_db.Name))
            output += $"[{nameof(port_db.Name)}: <u>{port_db.Name}</u> ]";
        output += $"[{nameof(port_db.PortNum)}: <u>{port_db.PortNum}</u> ]\n🔹🔹🔹";

        List<InlineKeyboardButton[]> kb_rows = new();
        kb_rows.Add(new[] { InlineKeyboardButton.WithCallbackData("К контроллеру", $"{GlobalStatic.Routes.AbPrefix}{port_db.Hardware!.Id}") });

        HttpResponseModel http_resp;
        HardwareGetHttpRequestModel hw_request = new() { HardwareId = port_db.Hardware!.Id };

        if (!string.IsNullOrEmpty(set_value))
        {
            hw_request.Path = $"?cmd={port_db.PortNum}:{set_value}";
            http_resp = await _hw.GetHardwareHtmlPage(hw_request, cancellation_token);
        }

        hw_request.Path = $"?pt={port_db.PortNum}";

        http_resp = await _hw.GetHardwareHtmlPage(hw_request, cancellation_token);
        if (!http_resp.IsSuccess)
        {
            output = http_resp.Message;
            return await _botClient.EditMessageTextAsync(chat_id, message_id, output, replyMarkup: new InlineKeyboardMarkup(kb_rows), cancellationToken: cancellation_token);
        }

        PortHtmlDomModel dom = new();
        await dom.Reload(http_resp.TextPayload);

        string _tg_resp_msg = dom.TelegramResponseHtmlRaw(port_db.Id);
        foreach ((string text, string href) lb in dom.Links)
            kb_rows.Add(new[] { InlineKeyboardButton.WithCallbackData($"🔗 {lb.text}", lb.href) });
        if (string.IsNullOrEmpty(set_value))
            return await _botClient.EditMessageTextAsync(chat_id, message_id, $"{output}\n{_tg_resp_msg}", replyMarkup: new InlineKeyboardMarkup(kb_rows), parseMode: ParseMode.Html, cancellationToken: cancellation_token);

        Message new_msg = await _botClient.SendTextMessageAsync(chat_id, $"{output}\n{_tg_resp_msg}", replyMarkup: new InlineKeyboardMarkup(kb_rows), parseMode: ParseMode.Html, cancellationToken: cancellation_token);
        await _botClient.DeleteMessageAsync(chat_id, message_id, cancellationToken: cancellation_token);
        return new_msg;
    }
}