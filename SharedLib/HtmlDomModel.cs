using System.Text.RegularExpressions;
using AngleSharp.Html.Parser;
using AngleSharp.Html.Dom;
using AngleSharp.Dom;

namespace SharedLib;

/// <summary>
/// 
/// </summary>
public class HtmlDomModel : HashSet<TreeItemDataModel>
{
    /// <summary>
    /// 
    /// </summary>
    public string? HtmlSource { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public async Task Reload(string? html_source)
    {
        HtmlSource = html_source;
        Clear();

        if (string.IsNullOrWhiteSpace(html_source))
            return;

        html_source = Regex.Replace(html_source ?? "", @"\s+", " ");
        HtmlParser parser = new();
        IHtmlDocument? Document = await parser.ParseDocumentAsync(html_source);
        if (Document.Body is not null)
            foreach (INode cn in Document.Body.ChildNodes)
            {
                if ((cn is IText && cn.Text().Trim().Equals("|")) || (cn is IHtmlAnchorElement && cn.Text().Trim().Equals("Back", StringComparison.OrdinalIgnoreCase)))
                    continue;

                TreeItemDataModel t = new(cn);

                if (cn.ChildNodes.Any())
                {
                    t.TreeItems = new();
                    InjectChilds(ref t, cn.ChildNodes);
                }

                Add(t);
            }

        while (this.FirstOrDefault()?.NodeName.Equals("br", StringComparison.OrdinalIgnoreCase) == true)
            Remove(this.First());
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
                t.TreeItems = new();
                InjectChilds(ref t, cn.ChildNodes);
            }
            nd.TreeItems!.Add(t);
        }
        while (nd.TreeItems?.FirstOrDefault()?.NodeName.Equals("br", StringComparison.OrdinalIgnoreCase) == true)
            nd.TreeItems.Remove(nd.TreeItems.First());
    }
}