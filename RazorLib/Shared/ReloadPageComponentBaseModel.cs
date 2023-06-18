using Microsoft.AspNetCore.Components;
using RazorLib.Shared.hardwares;
using SharedLib.IServices;
using RazorLib.Shared;
using SharedLib;

namespace RazorLib
{
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
        protected ShowMessagesComponent? showMessages;

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
            HttpResponseModel rest = await _hardwares.GetHardwareHtmlPage(new HardvareGetRequestModel() { HardwareId = Id, Path = PagePath });

            if (!rest.IsSuccess)
            {
                showMessages?.ShowMessages(rest.Messages);
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
}