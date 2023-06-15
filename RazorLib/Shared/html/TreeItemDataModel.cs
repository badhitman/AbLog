using AngleSharp.Common;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Newtonsoft.Json;
using static MudBlazor.FilterOperator;

namespace RazorLib.Shared.html
{
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

                //foreach (IAttr at in Attributes)
                //{
                //    _t += $" [{at.Name}:{at.Value}];";
                //}

                return _t;
            }
        }

        /// <summary>
        /// Node name (HTML)
        /// </summary>
        public string NodeName { get; set; }

        /// <summary>
        /// Text content (HTML)
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// Attributes (DOM HTML)
        /// </summary>
        public INamedNodeMap? Attributes { get; set; }

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
        public HashSet<TreeItemDataModel>? TreeItems { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TreeItemDataModel(INode cn)
        {
            NodeName = cn.NodeName;
            if (cn is IText tn)
            {
                Text = tn.Text();
            }
            else if (cn is IHtmlOptionElement oe)
            {
                Attributes = oe.Attributes;
                Text = oe.Text();
            }
            else if (cn is IHtmlSelectElement se)
            {
                Attributes = se.Attributes;
            }
            else if (cn is IHtmlInputElement ie)
            {
                Attributes = ie.Attributes;
            }
            else if (cn is IHtmlFormElement fe)
            {
                Attributes = fe.Attributes;
                Number = fe.ChildElementCount;
            }
            else if (cn is IHtmlAnchorElement ae)
            {
                Attributes = ae.Attributes;
                Number = ae.ChildElementCount;
                Text = Number > 0 ? "" : cn.Text();
            }
            else if (cn is IHtmlBreakRowElement)
            {

            }
        }
    }
}
