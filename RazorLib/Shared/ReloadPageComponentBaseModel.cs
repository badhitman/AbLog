////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using RazorLib.Shared.hardwires;
using SharedLib;

namespace RazorLib;

/// <summary>
/// Reload page component (base)
/// </summary>
public abstract class ReloadPageComponentBaseModel : BlazorBusyComponentBaseModel
{
    /// <inheritdoc/>
    [Inject]
    public required IHardwiresService Hardwires { get; set; }

    /// <inheritdoc/>
    [Inject]
    public required ISnackbar Snackbar { get; set; }

    /// <inheritdoc/>
    [CascadingParameter, EditorRequired]
    public int Id { get; set; }

    /// <inheritdoc/>
    [CascadingParameter(Name = "PagePath")]
    public required string PagePath { get; set; }

    /// <inheritdoc/>
    protected HardwiresNavPillsComponent? refHtmlRoot;

    /// <inheritdoc/>
    protected string? HtmlSource;

    /// <summary>
    /// Reload page
    /// </summary>
    protected async void ReloadPage()
    {
        await GetData();
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
        HttpResponseModel rest = await Hardwires.GetHardwareHtmlPage(new HardwareGetHttpRequestModel() { HardwareId = Id, Path = PagePath });

        if (!rest.IsSuccess)
        {
            Snackbar.ShowMessagesResponse(rest.Messages);
            IsBusyProgress = false;
            StateHasChanged();
            return;
        }

        HtmlSource = rest.Response;
        IsBusyProgress = false;
        StateHasChanged();
        if (!IsUpdated && refHtmlRoot?.refHtmlRoot is not null)
        {
            IsUpdated = true;
            refHtmlRoot.refHtmlRoot.StateHasChangedCall(HtmlSource);
        }
    }
}