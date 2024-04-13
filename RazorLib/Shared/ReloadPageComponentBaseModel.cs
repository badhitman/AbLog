////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using RazorLib.Shared.hardwares;
using SharedLib;

namespace RazorLib;

/// <summary>
/// Reload page component (base)
/// </summary>
public abstract class ReloadPageComponentBaseModel : BlazorBusyComponentBaseModel
{
    /// <inheritdoc/>
    [Inject]
    public required IHardwaresService Hardwares { get; set; }

    /// <inheritdoc/>
    [Inject]
    public required ISnackbar Snackbar { get; set; }

    /// <inheritdoc/>
    [CascadingParameter, EditorRequired]
    public int Id { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required string PagePath { get; set; }

    /// <inheritdoc/>
    protected HardwaresNavPillsComponent? refHtmlRoot;

    /// <inheritdoc/>
    protected string? HtmlSource;

    /// <summary>
    /// Reload page
    /// </summary>
    protected void ReloadPage()
    {
        InvokeAsync(async () => await GetData());
    }

    bool IsUpdated = false;

    /// <inheritdoc/>
    protected async Task GetData()
    {
        if (Id <= 0)
            return;
        IsUpdated = false;
        IsBusyProgress = true;
        StateHasChanged();
        HttpResponseModel rest = await Hardwares.GetHardwareHtmlPage(new HardwareGetHttpRequestModel() { HardwareId = Id, Path = PagePath });

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