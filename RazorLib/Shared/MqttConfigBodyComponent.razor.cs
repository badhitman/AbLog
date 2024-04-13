using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using SharedLib;

namespace RazorLib.Shared;

/// <summary>
/// Настройки MQTT транспорта
/// </summary>
public partial class MqttConfigBodyComponent : BlazorBusyComponentBaseModel
{
    /// <summary>
    /// Parameters storage service
    /// </summary>
    [Inject]
    public required IParametersStorageService ParametersStorageService { get; set; }

    /// <summary>
    /// Tools service
    /// </summary>
    [Inject]
    public required IToolsService ToolsService { get; set; }

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


    MqttConfigModel conf = new() { Password = "", Secret = "", Server = "", Username = "" };
    MqttConfigModel conf_self = new() { Password = "", Secret = "", Server = "", Username = "" };
    private EditContext? editContext;
    bool IsEdit => conf != conf_self;
    bool ShowSecret = false;
    bool ShowPassword = false;

    bool ServiceIsRunning = false;

    private Snackbar? _snackbar;
    void MqttDebug((TimeSpan Duration, string? Message, string TopicName) sender)
    {
        var config = (SnackbarOptions options) =>
        {
            options.DuplicatesBehavior = SnackbarDuplicatesBehavior.Prevent;
        };

        if (sender.Message is null && sender.Duration.TotalSeconds > 5)
        {
            if (_snackbar is not null)
                SnackBar.Remove(_snackbar);
            _snackbar = SnackBar.Add($"Задержка {sender.TopicName}: {sender.Duration}", Severity.Normal, configure: config, key: "mudblazor");
        }
    }

    /// <inheritdoc/>
    public override void Dispose()
    {
        NotifyService.MqttDebugNotify -= MqttDebug;
        GC.SuppressFinalize(this);
        base.Dispose();
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        NotifyService.MqttDebugNotify += MqttDebug;
        editContext = new(conf);
        IsBusyProgress = true;
        MqttConfigResponseModel res = await ParametersStorageService.GetMqttConfig(this.CancellationToken);
        SnackBar.ShowMessagesResponse(res.Messages);
        if (!res.IsSuccess || res.Conf is null)
        {
            IsBusyProgress = false;
            return;
        }
        conf = res.Conf;
        editContext = new(conf);
        conf_self = GlobalStatic.CreateDeepCopy(conf);

        await GetStatusService();

        IsBusyProgress = false;
    }

    async Task GetStatusService()
    {
        BoolResponseModel status = await ToolsService.StatusMqtt();
        ServiceIsRunning = status.Response;
        SnackBar.ShowMessagesResponse(status.Messages);
    }

    async Task StopService()
    {
        IsBusyProgress = true;
        ResponseBaseModel stop_res = await ToolsService.StopMqtt(this.CancellationToken);
        SnackBar.ShowMessagesResponse(stop_res.Messages);
        await GetStatusService();
        IsBusyProgress = false;
    }

    async Task RestartService()
    {
        IsBusyProgress = true;
        ResponseBaseModel start_res = await ToolsService.StartMqtt(this.CancellationToken);
        SnackBar.ShowMessagesResponse(start_res.Messages);
        await GetStatusService();
        IsBusyProgress = false;
    }

    async Task SaveConfMqtt()
    {
        bool? validate = editContext?.Validate();
        if (validate != true)
        {
            SnackBar.Add("Форма не заполнена!", Severity.Warning, conf => conf.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
            return;
        }

        IsBusyProgress = true;
        ResponseBaseModel res = await ParametersStorageService.SaveMqttConfig(conf, this.CancellationToken);
        SnackBar.ShowMessagesResponse(res.Messages);

        if (res.IsSuccess)
            conf_self = GlobalStatic.CreateDeepCopy(conf);

        IsBusyProgress = false;
    }

    async Task TestConnect()
    {
        IsBusyProgress = true;
        ResponseBaseModel res = await ToolsService.TestMqttConnect(conf);
        SnackBar.ShowMessagesResponse(res.Messages);
        IsBusyProgress = false;
    }
}