////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using RazorLib.Shared.hardwares;
using SharedLib;

namespace RazorLib;

/// <summary>
/// 
/// </summary>
public abstract class ReloadPageComponentBaseModel : BlazorBusyComponentBaseModel
{
    /// <summary>
    /// 
    /// </summary>
    [Inject]
    protected IHardwaresService _hardwares { get; set; } = default!;

    /// <summary>
    /// 
    /// </summary>
    [Inject]
    protected ISnackbar Snackbar { get; set; } = default!;

    /// <summary>
    /// 
    /// </summary>
    [CascadingParameter, EditorRequired]
    public int Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public string PagePath { get; set; } = default!;

    /// <summary>
    /// 
    /// </summary>
    protected HardwaresNavPillsComponent? refHtmlRoot;

    /// <summary>
    /// 
    /// </summary>
    protected string? HtmlSource;

    /// <summary>
    /// 
    /// </summary>
    protected void ReloadPage()
    {
        InvokeAsync(async () => await GetData());
    }

    bool IsUpdated = false;

    /// <summary>
    /// 
    /// </summary>
    protected async Task GetData()
    {
        if (Id <= 0)
            return;
        IsUpdated = false;
        IsBusyProgress = true;
        StateHasChanged();
        HttpResponseModel rest = await _hardwares.GetHardwareHtmlPage(new HardwareGetHttpRequestModel() { HardwareId = Id, Path = PagePath });

        if (!rest.IsSuccess)
        {
            Snackbar.ShowMessagesResponse(rest.Messages);
            IsBusyProgress = false;
            StateHasChanged();
            return;
        }

        HtmlSource = rest.TextPayload;
        IsBusyProgress = false;
        StateHasChanged();
        if (!IsUpdated && refHtmlRoot?.refHtmlRoot is not null)
        {
            IsUpdated = true;
            refHtmlRoot.refHtmlRoot.StateHasChangedCall(HtmlSource);
        }
    }
}