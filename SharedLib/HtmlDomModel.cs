////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System.Text.RegularExpressions;

namespace SharedLib;

/// <summary>
/// HTML Dom
/// </summary>
public partial class HtmlDomModel : HashSet<HtmlDomTreeItemDataModel>
{
    /// <summary>
    /// HTML исходники
    /// </summary>
    public string? HtmlSource { get; private set; }

    /// <summary>
    /// Перезагрузка
    /// </summary>
    public async Task Reload(string? html_source)
    {
        HtmlSource = html_source;
        Clear();

        if (string.IsNullOrWhiteSpace(html_source))
            return;

        html_source = SpicesMyRegex().Replace(html_source ?? "", " ");
        html_source = SpicesBeforeStartTagMyRegex().Replace(html_source ?? "", "<");
        html_source = SpicesAfterStartTagMyRegex().Replace(html_source ?? "", "<");
        html_source = SpicesBeforeEndTagMyRegex().Replace(html_source ?? "", ">");
        html_source = SpicesAfterEndTagMyRegex().Replace(html_source ?? "", ">");

        HtmlParser parser = new();
        IHtmlDocument? Document = await parser.ParseDocumentAsync(html_source);
        if (Document.Body is not null)
            foreach (INode cn in Document.Body.ChildNodes)
            {
                if ((cn is IText && cn.Text().Trim().Equals("|")) || (cn is IHtmlAnchorElement && cn.Text().Trim().Equals("Back", StringComparison.OrdinalIgnoreCase)))
                    continue;

                HtmlDomTreeItemDataModel t = new(cn);

                if (cn.ChildNodes.Length != 0)
                {
                    t.TreeItems = [];
                    InjectChilds(ref t, cn.ChildNodes);
                }

                Add(t);
            }

        while (this.FirstOrDefault()?.NodeName.Equals("br", StringComparison.OrdinalIgnoreCase) == true)
            Remove(this.First());
    }

    static void InjectChilds(ref HtmlDomTreeItemDataModel nd, INodeList nls)
    {
        foreach (INode cn in nls)
        {
            if ((cn is IText && cn.Text().Trim().Equals("|")) || (cn is IHtmlAnchorElement && cn.Text().Trim().Equals("Back", StringComparison.OrdinalIgnoreCase)))
                continue;

            HtmlDomTreeItemDataModel t = new(cn) { Parent = nd };
            if (cn.ChildNodes.Length != 0)
            {
                t.TreeItems = [];
                InjectChilds(ref t, cn.ChildNodes);
            }
            nd.TreeItems!.Add(t);
        }
        while (nd.TreeItems?.FirstOrDefault()?.NodeName.Equals("br", StringComparison.OrdinalIgnoreCase) == true)
            nd.TreeItems.Remove(nd.TreeItems.First());
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex SpicesMyRegex();

    [GeneratedRegex(@"\s+<")]
    private static partial Regex SpicesBeforeStartTagMyRegex();

    [GeneratedRegex(@"<\s+")]
    private static partial Regex SpicesAfterStartTagMyRegex();

    [GeneratedRegex(@"\s+>")]
    private static partial Regex SpicesBeforeEndTagMyRegex();

    [GeneratedRegex(@">\s+")]
    private static partial Regex SpicesAfterEndTagMyRegex();
}