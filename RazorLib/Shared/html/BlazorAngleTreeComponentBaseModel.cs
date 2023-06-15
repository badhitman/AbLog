using Microsoft.AspNetCore.Components;
using System.Text.RegularExpressions;
using AngleSharp.Html.Parser;
using RazorLib.Shared.html;
using AngleSharp.Html.Dom;
using AngleSharp.Dom;

namespace RazorLib
{
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
        public string? Html { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected IHtmlDocument? Document;

        /// <summary>
        /// 
        /// </summary>
        protected HashSet<TreeItemDataModel> TreeItems { get; set; } = new HashSet<TreeItemDataModel>();

        /// <inheritdoc/>
        public void StateHasChangedCall(string? html)
        {
            Html = html;
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
            TreeItems.Clear();
            IsBusyProgress = false;
            Html = Regex.Replace(Html ?? "", @"\s+", " ");
            HtmlParser parser = new();
            Document = await parser.ParseDocumentAsync(Html);
            if (Document.Body is not null)
                foreach (INode cn in Document.Body.ChildNodes)
                {
                    if ((cn is IText && cn.Text().Trim().Equals("|")) || cn is IHtmlBreakRowElement)
                        continue;

                    TreeItemDataModel t = new(cn);

                    if (cn.ChildNodes.Any())
                    {
                        t.TreeItems = new HashSet<TreeItemDataModel>();
                        InjectChilds(ref t, cn.ChildNodes);
                    }

                    TreeItems.Add(t);
                }
            IsBusyProgress = false;
        }

        /// <summary>
        /// 
        /// </summary>
        protected void InjectChilds(ref TreeItemDataModel nd, INodeList nls)
        {
            foreach (INode cn in nls)
            {
                if ((cn is IText && cn.Text().Trim().Equals("|")) || cn is IHtmlBreakRowElement)
                    continue;

                TreeItemDataModel t = new(cn);
                if (cn.ChildNodes.Any())
                {
                    t.TreeItems = new HashSet<TreeItemDataModel>();
                    InjectChilds(ref t, cn.ChildNodes);
                }
                nd.TreeItems!.Add(t);
            }
        }
    }
}