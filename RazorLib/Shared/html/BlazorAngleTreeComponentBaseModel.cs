////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using AngleSharp.Html.Dom;
using SharedLib;

namespace RazorLib;

/// <summary>
/// Blazor Angle-tree
/// </summary>
public abstract class BlazorAngleTreeComponentBaseModel : BlazorBusyComponentBaseModel
{
    /// <inheritdoc/>
    [CascadingParameter, EditorRequired]
    public int Id { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public string? HtmlSource { get; set; }

    /// <inheritdoc/>
    protected IHtmlDocument? Document;

    /// <inheritdoc/>
    protected HtmlDomModel TreeItems { get; set; } = [];

    /// <summary>
    /// Обновляет код разметки, парсит данные (AngleHtml) и вызывает StateHasChanged.
    /// </summary>
    public void StateHasChangedCall(string? html)
    {
        HtmlSource = html;
        InvokeAsync(async () => await Parse());
        base.StateHasChangedCall();
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await Parse();
    }

    async Task Parse()
    {
        IsBusyProgress = false;
        await TreeItems.Reload(HtmlSource);
        IsBusyProgress = false;
    }
}