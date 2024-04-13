////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace SharedLib;

/// <summary>
/// HTML Dom tree item Data
/// </summary>
public class HtmlDomTreeItemDataModel
{
    /// <inheritdoc/>
    public string Title
    {
        get
        {
            string _t = $"{Text} <{NodeName}/>"; ;

            if (Attributes is not null && Attributes.Length != 0)
                foreach (KeyValuePair<string, string?> at in Attributes)
                    _t += $" [{at.Key}:{at.Value}];";

            return _t.Trim();
        }
    }

    /// <summary>
    /// Node name (HTML)
    /// </summary>
    public string NodeName { get; set; }

    /// <summary>
    /// Node HTML
    /// </summary>
    public string NodeHtml { get; set; }

    /// <inheritdoc/>
    public HtmlDomTreeItemDataModel? Parent { get; set; }

    /// <summary>
    /// Text content (HTML)
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// Attributes (DOM HTML)
    /// </summary>
    public KeyValuePair<string, string?>[]? Attributes { get; set; }

    /// <summary>
    /// Child elements count
    /// </summary>
    public int? ChildElementsCount { get; set; }

    /// <summary>
    /// Развёрнут
    /// </summary>
    public bool IsExpanded { get; set; }

    /// <summary>
    /// Элементы дерева
    /// </summary>
    public HtmlDomModel? TreeItems { get; set; }

    /// <summary>
    /// HTML Dom tree item Data
    /// </summary>
    public HtmlDomTreeItemDataModel(INode cn)
    {
        NodeName = cn.NodeName.ToLower().Trim();
        NodeHtml = cn.ToHtml().Trim();

        if (cn is IText tn)
        {
            Text = tn.Text().Trim();
        }
        else if (cn is IHtmlOptionElement oe)
        {
            Attributes = oe.Attributes.Select(x => new KeyValuePair<string, string?>(x.Name.Trim(), x.Value.Trim())).ToArray();
            Text = oe.Text().Trim();
        }
        else if (cn is IHtmlSelectElement se)
        {
            Attributes = se.Attributes.Select(x => new KeyValuePair<string, string?>(x.Name.Trim(), x.Value.Trim())).ToArray();
        }
        else if (cn is IHtmlInputElement ie)
        {
            Attributes = ie.Attributes.Select(x => new KeyValuePair<string, string?>(x.Name.Trim(), x.Value.Trim())).ToArray();
        }
        else if (cn is IHtmlFormElement fe)
        {
            Attributes = fe.Attributes.Select(x => new KeyValuePair<string, string?>(x.Name.Trim(), x.Value.Trim())).ToArray();
            ChildElementsCount = fe.ChildElementCount;
        }
        else if (cn is IHtmlAnchorElement ae)
        {
            Attributes = ae.Attributes.Select(x => new KeyValuePair<string, string?>(x.Name.Trim(), x.Value.Trim())).ToArray();
            ChildElementsCount = ae.ChildElementCount;
            Text = ChildElementsCount > 0 ? "" : cn.Text().Trim();
        }
        else if (cn is IHtmlBreakRowElement)
        {

        }
        else
        {
            throw new Exception($"cn.GetType().FullName. error {{DB26CDE8-BFE5-4945-BD9A-7DA7C0AD3985}}");
        }
    }
}