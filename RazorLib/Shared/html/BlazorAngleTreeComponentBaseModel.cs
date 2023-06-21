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
        public string? HtmlSource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected IHtmlDocument? Document;

        /// <summary>
        /// 
        /// </summary>
        protected HashSet<TreeItemDataModel> TreeItems { get; set; } = new HashSet<TreeItemDataModel>();

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
            TreeItems.Clear();
            IsBusyProgress = false;
            HtmlSource = Regex.Replace(HtmlSource ?? "", @"\s+", " ");
            HtmlParser parser = new();
            Document = await parser.ParseDocumentAsync(HtmlSource);
            if (Document.Body is not null)
                foreach (INode cn in Document.Body.ChildNodes)
                {
                    if ((cn is IText && cn.Text().Trim().Equals("|")) || (cn is IHtmlAnchorElement && cn.Text().Trim().Equals("Back", StringComparison.OrdinalIgnoreCase)))
                        continue;

                    TreeItemDataModel t = new(cn);

                    if (cn.ChildNodes.Any())
                    {
                        t.TreeItems = new HashSet<TreeItemDataModel>();
                        InjectChilds(ref t, cn.ChildNodes);
                    }

                    TreeItems.Add(t);
                }

            while (TreeItems.FirstOrDefault()?.NodeName.Equals("br", StringComparison.OrdinalIgnoreCase) == true)
                TreeItems.Remove(TreeItems.First());

            IsBusyProgress = false;
        }

        void InjectChilds(ref TreeItemDataModel nd, INodeList nls)
        {
            foreach (INode cn in nls)
            {
                if ((cn is IText && cn.Text().Trim().Equals("|")) || (cn is IHtmlAnchorElement && cn.Text().Trim().Equals("Back", StringComparison.OrdinalIgnoreCase)))
                    continue;

                TreeItemDataModel t = new(cn) { Parent = nd };
                if (cn.ChildNodes.Any())
                {
                    t.TreeItems = new HashSet<TreeItemDataModel>();
                    InjectChilds(ref t, cn.ChildNodes);
                }
                nd.TreeItems!.Add(t);
            }
            while (nd.TreeItems?.FirstOrDefault()?.NodeName.Equals("br", StringComparison.OrdinalIgnoreCase) == true)
                nd.TreeItems.Remove(nd.TreeItems.First());
        }
    }
}