using Microsoft.AspNetCore.Components;
using SharedLib;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Web;

namespace RazorLib.Shared.hardwares;

/// <summary>
/// 
/// </summary>
public partial class HardwareNavButtonComponent : ReloadPageComponentBaseModel
{
    /// <summary>
    /// Client config
    /// </summary>
    [Inject]
    public required ClientConfigModel Conf { get; set; }

    /// <summary>
    /// Text
    /// </summary>
    [Parameter, EditorRequired]
    public required string Text { get; set; }

    /// <summary>
    /// Автоматически раскрыть/загрузить данные
    /// </summary>
    [Parameter]
    public bool AutoLoadAndExpand { get; set; } = false;

    bool IsExpand = false;
    readonly CancellationToken cancellationToken = new();
    EntryModel PortEntry = new();

    /// <summary>
    /// Expand
    /// </summary>
    public async Task Expand()
    {
        IsExpand = !IsExpand;
        if (!IsExpand)
            return;

        await GetData();
    }
    static readonly SemaphoreSlim _syncLock = new(5, 5);

    static Regex port_num = new(@"^\d+$", RegexOptions.Compiled);

    /// <summary>
    /// Update port-name
    /// </summary>
    protected async Task UpdatePortName()
    {
        ResponseBaseModel rest = await _hardwares.SetNamePort(PortEntry);

        Snackbar.ShowMessagesResponse(rest.Messages);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        string ext_path = PagePath[(PagePath.IndexOf("?") + 1)..];
        NameValueCollection path_parameters = HttpUtility.ParseQueryString(ext_path);
        if (path_parameters.Count == 1 && port_num.IsMatch(path_parameters.Get("pt") ?? ""))
        {
            await InvokeAsync(async () =>
            {
                IsBusyProgress = true;
                StateHasChanged();
                await _syncLock.WaitAsync();

                EntriyResponseModel rest = await _hardwares.CheckPortHardware(new PortHardwareCheckRequestModel() { PortNum = uint.Parse(path_parameters.Get("pt")!), HardwareId = Id, CreatePortIfNoptExist = true }, cancellationToken);

                Snackbar.ShowMessagesResponse(rest.Messages);
                if (rest.IsSuccess)
                {
                    PortEntry = rest.Entry ?? new();

                    if (AutoLoadAndExpand)
                        await Expand();
                }

                _syncLock.Release();
                IsBusyProgress = false;
                StateHasChanged();
            });
        }
    }
    /// <inheritdoc/>

    public void Dispose()
    {
        cancellationToken.ThrowIfCancellationRequested();
        GC.SuppressFinalize(this);
    }
}