using AngleSharp.Html.Dom;
using AngleSharp.Dom;
using AngleSharp;

namespace SharedLib;

/// <summary>
/// 
/// </summary>
public class TreeItemDataModel
{
    /// <summary>
    /// 
    /// </summary>
    public string Title
    {
        get
        {
            string _t = $"{Text} <{NodeName}/>"; ;

            if (Attributes?.Any() == true)
                foreach (KeyValuePair<string, string?> at in Attributes)
                {
                    _t += $" [{at.Key}:{at.Value}];";
                }

            return _t.Trim();
        }
    }

    /// <summary>
    /// Node name (HTML)
    /// </summary>
    public string NodeName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string NodeHtml { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public TreeItemDataModel? Parent { get; set; }

    /// <summary>
    /// Text content (HTML)
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// Attributes (DOM HTML)
    /// </summary>
    public KeyValuePair<string, string?>[]? Attributes { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int? Number { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool IsExpanded { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public HtmlDomModel? TreeItems { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public TreeItemDataModel(INode cn)
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
            Number = fe.ChildElementCount;
        }
        else if (cn is IHtmlAnchorElement ae)
        {
            Attributes = ae.Attributes.Select(x => new KeyValuePair<string, string?>(x.Name.Trim(), x.Value.Trim())).ToArray();
            Number = ae.ChildElementCount;
            Text = Number > 0 ? "" : cn.Text().Trim();
        }
        else if (cn is IHtmlBreakRowElement)
        {

        }
        else
        {
            throw new Exception(cn.GetType().FullName);
        }
    }
}