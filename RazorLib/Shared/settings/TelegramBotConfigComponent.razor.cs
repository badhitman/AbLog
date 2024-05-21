using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json;
using SharedLib;

namespace RazorLib.Shared.settings;

/// <summary>
/// Telegram Bot configuration - component
/// </summary>
public partial class TelegramBotConfigComponent : BlazorBusyComponentBaseModel, IDisposable
{
    /// <summary>
    /// Storage
    /// </summary>
    [Inject]
    public required IParametersStorageService Storage { get; set; }

    /// <summary>
    /// Tools
    /// </summary>
    [Inject]
    public required IToolsService Tools { get; set; }

    /// <summary>
    /// Snackbar
    /// </summary>
    [Inject]
    public required ISnackbar SnackBar { get; set; }

    /// <summary>
    /// Notify service
    /// </summary>
    [Inject]
    public required INotifyService NotifyService { get; set; }

    TelegramBotConfigModel _conf = new();
    TelegramBotConfigModel _conf_self = new();
    bool ShowToken = false;
    bool CanSave => _conf != _conf_self;

    void ResetForm()
    {
        _conf = GlobalStatic.CreateDeepCopy(_conf_self);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        NotifyService.Notify += CheckTelegramUserHandleNotify;
        IsBusyProgress = true;
        TResponseModel<TelegramBotConfigModel> _bot_conf = await Storage.GetTelegramBotConfig(CancellationToken);
        IsBusyProgress = false;
        SnackBar.ShowMessagesResponse(_bot_conf.Messages);

        _conf = _bot_conf.Response ?? new();
        _conf_self = GlobalStatic.CreateDeepCopy(_conf);
        StateHasChanged();
    }

    async Task TestTelegramBotConfig()
    {
        TelegramBotCheckResponseModel _conf_bot = await Tools.TestTelegramBotConnect(_conf);
        SnackBar.ShowMessagesResponse(_conf_bot.Messages);
        if (_conf_bot.Id > 0)
        {
            SnackBar.Add($"[{nameof(_conf_bot.Id)}:{_conf_bot.Id}]\n[{nameof(_conf_bot.FirstName)}:{_conf_bot.FirstName}]\n[{nameof(_conf_bot.LastName)}:{_conf_bot.LastName}]\n[{nameof(_conf_bot.Username)}:{_conf_bot.Username}]", Severity.Info, conf => conf.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
            if (_conf_bot.ServiceIsRunning is null)
            {
                SnackBar.Add("Служба не запущена. Запуск службы выполняется при старте приложения (если включён автостарт бота). Остановить бота можно перезапуском приложения (предварительно отключите автостарт)", Severity.Normal, conf => conf.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
            }
            else
            {
                SnackBar.Add($"Служба запущена:\n[{nameof(_conf_bot.ServiceIsRunning.FirstName)}:{_conf_bot.ServiceIsRunning.FirstName}]\n[{nameof(_conf_bot.ServiceIsRunning.LastName)}:{_conf_bot.ServiceIsRunning.LastName}]\n[{nameof(_conf_bot.ServiceIsRunning.Username)}:{_conf_bot.ServiceIsRunning.Username}]\n[{nameof(_conf_bot.ServiceIsRunning.Id)}:{_conf_bot.ServiceIsRunning.Id}]", Severity.Success, conf => conf.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
            }
        }
        else
        {
            SnackBar.Add("Ошибка. Токен не прошёл проверку.", Severity.Error, conf => conf.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
        }
        StateHasChanged();
    }

    /// <inheritdoc/>
    async Task OnValidSubmitHandle()
    {
        IsBusyProgress = true;
        ResponseBaseModel rest = await Storage.SaveTelegramBotConfig(_conf, CancellationToken);
        SnackBar.ShowMessagesResponse(rest.Messages);
        IsBusyProgress = false;

        if (!rest.IsSuccess)
            return;

        _conf_self = GlobalStatic.CreateDeepCopy(_conf);
        IsBusyProgress = false;
    }

    void CheckTelegramUserHandleNotify(TResponseModel<UserModelDB> user)
    {
        if (user.Response is null)
        {
            SnackBar.Add($"error {{8B0B5C84-3FAC-4289-97A8-096AD5023C38}} User is null", Severity.Error, c => c.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
            return;
        }

        if (user.Response.IsDisabled)
        {
            SnackBar.Add($"Сообщение Telegram отключённого пользователя:\n{JsonConvert.SerializeObject(user, Formatting.Indented)}", Severity.Normal, c => c.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
        }
        else
        {
            SnackBar.Add($"Сообщение Telegram:\n{JsonConvert.SerializeObject(new { user.Response.Name, user.Response.FirstName, user.Response.LastName, user.Response.AlarmSubscriber, user.Response.AllowChangeMqttConfig, user.Response.AllowSystemCommands, user.Response.CommandsAllowed, user.Response.Email }, Formatting.Indented)}", Severity.Info, c => c.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
        }
    }


    /// <inheritdoc/>
    public override void Dispose()
    {
        NotifyService.Notify -= CheckTelegramUserHandleNotify;
        base.Dispose();
        GC.SuppressFinalize(this);
    }
}