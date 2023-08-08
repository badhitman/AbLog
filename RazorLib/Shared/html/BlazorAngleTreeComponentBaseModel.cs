////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using AngleSharp.Html.Dom;
using SharedLib;

namespace RazorLib;

/// <summary>
/// 
/// </summary>
public abstract class BlazorAngleTreeComponentBaseModel : BlazorBusyComponentBaseModel
{
    /// <summary>
    /// 
    /// </summary>
    [CascadingParameter, EditorRequired]
    public int Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter, EditorRequired]
    public string? HtmlSource { get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected IHtmlDocument? Document;

    /// <summary>
    /// 
    /// </summary>
    protected HtmlDomModel TreeItems { get; set; } = new();

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