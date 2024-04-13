////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using System.Collections.Specialized;
using System.Web;

namespace SharedLib;

/// <summary>
/// Порт устройства
/// </summary>
public class PortHtmlDomModel : HtmlDomModel
{
    /// <inheritdoc/>
    public List<(string text, string href)> Links = [];

    string last_tag_name = "";
    int _port_id;
    private static readonly string[] attributes_html = ["submit", "hidden"];

    /// <summary>
    /// Получить данные по порту
    /// </summary>
    public string TelegramResponseHtmlRaw(int port_id)
    {
        Links.Clear();
        _port_id = port_id;
        if (!this.Any())
            return "данные отсутсвуют. error {9EDCD529-B1F4-45F5-9405-60AA5C2FB30B}";

        last_tag_name = "";
        string res = "";
        foreach (HtmlDomTreeItemDataModel item in this)
            ReadNode(ref res, item);

        return res;
    }

    void ReadNode(ref string tg_resp_html, HtmlDomTreeItemDataModel item, int deep_num = 0)
    {
        string _pref = "";
        for (int i = 0; i < deep_num; i++)
            _pref += "○";
        if (!string.IsNullOrEmpty(_pref))
            _pref = $"{_pref} ";

        switch (item.NodeName.ToLower())
        {
            case "select":
                ReadSelectNode(ref tg_resp_html, item, _pref);
                break;
            case "br":
                break;
            case "a":
                string? href = item.Attributes?.FirstOrDefault(x => x.Key.Equals("href", StringComparison.OrdinalIgnoreCase)).Value;
                if (href is not null && item.Text is not null)
                {
                    if (href.Contains("pt=", StringComparison.OrdinalIgnoreCase) && href.Contains("cmd=", StringComparison.OrdinalIgnoreCase))
                    {
                        href = href[(href.IndexOf('?') + 1)..];
                        NameValueCollection parse_query = HttpUtility.ParseQueryString(href);
                        string? cmd_key = parse_query.AllKeys.FirstOrDefault(x => x?.Equals("cmd", StringComparison.OrdinalIgnoreCase) == true);
                        string? cmd_val = parse_query.Get(cmd_key);
                        if (cmd_val is not null)
                        {
                            string[] cmd_sections = cmd_val.Split(":");
                            Links.Add((item.Text, $"{GlobalStatic.Routes.Port}:{_port_id}:{cmd_sections.LastOrDefault()}"));
                        }
                    }
                }
                break;
            case "form":
                if (item.TreeItems?.Any() == true)
                {
                    last_tag_name = "";
                    deep_num++;
                    foreach (HtmlDomTreeItemDataModel sub_item in item.TreeItems.Where(x => !x.NodeName.Equals("br", StringComparison.OrdinalIgnoreCase)))
                        ReadNode(ref tg_resp_html, sub_item, deep_num);
                }
                break;
            case "input":
                string? input_type = item.Attributes?.FirstOrDefault(x => x.Key.Equals("type", StringComparison.OrdinalIgnoreCase)).Value;
                string? input_name = item.Attributes?.FirstOrDefault(x => x.Key.Equals("name", StringComparison.OrdinalIgnoreCase)).Value;
                string? input_value = item.Attributes?.FirstOrDefault(x => x.Key.Equals("value", StringComparison.OrdinalIgnoreCase)).Value;

                if (attributes_html.Any(x => x.Equals(input_type, StringComparison.OrdinalIgnoreCase)))
                    break;

                if (input_type?.Equals("checkbox", StringComparison.OrdinalIgnoreCase) == true)
                {
                    bool is_checked = item.Attributes?.Any(x => x.Key.Equals("checked", StringComparison.OrdinalIgnoreCase)) == true;
                    if (last_tag_name.Equals("#text"))
                        tg_resp_html = $"{tg_resp_html.TrimEnd()}";
                    else
                        tg_resp_html += $"\n{_pref}";

                    tg_resp_html += $"{item.Text ?? $" <u>{input_name}</u> {(is_checked ? "✅" : "▢")} <i>({input_value})</i>"}\n";
                }
                else
                {
                    tg_resp_html = $"{tg_resp_html.TrimEnd()} {item.Text ?? $""} <u>{input_name}</u> 🔠 {(string.IsNullOrWhiteSpace(input_value) ? "<i>(-not set-)</i>" : $"<code>{input_value}</code>")}";
                }
                break;
            default:
                tg_resp_html += $"{_pref}{item.Text ?? $"-{item.NodeName}-"}\n";
                if (item.Attributes is not null && item.Attributes.Length != 0)
                    tg_resp_html = $"{tg_resp_html.TrimEnd()} {$"({string.Join(",", item.Attributes.Select(x => $"[{x.Key}:'{x.Value}']"))})"}\n";

                if (item.TreeItems?.Any() == true)
                {
                    deep_num++;
                    last_tag_name = "";
                    foreach (HtmlDomTreeItemDataModel sub_item in item.TreeItems)
                        ReadNode(ref tg_resp_html, sub_item, deep_num);
                }
                if (tg_resp_html.TrimEnd().EndsWith('○'))
                    tg_resp_html = $"{tg_resp_html[..tg_resp_html.LastIndexOf('○')]}\n";
                break;
        }
        last_tag_name = item.NodeName;
    }

    static void ReadSelectNode(ref string tg_resp_html, HtmlDomTreeItemDataModel item, string _pref)
    {
        string selected_val = GetSelectedItem(item.TreeItems);

        if (!string.IsNullOrEmpty(selected_val))
            tg_resp_html = $"{tg_resp_html.TrimEnd()} <u>{item.Attributes?.FirstOrDefault(x => x.Key.Equals("name", StringComparison.OrdinalIgnoreCase)).Value}</u> 📚 {selected_val}\n";
    }

    static string GetSelectedItem(HtmlDomModel? treeItems)
    {
        if (treeItems?.Any() != true)
            return "";

        string? value_prop = null; ;
        foreach (HtmlDomTreeItemDataModel item in treeItems)
        {
            if (item.Attributes?.Any(x => x.Key.Equals("selected", StringComparison.OrdinalIgnoreCase)) == true)
            {
                value_prop = item.Attributes.FirstOrDefault(x => x.Key.Equals("value", StringComparison.OrdinalIgnoreCase)).Value;
                return $"'<code>{item.Text}</code>'";
            }
        }

        return value_prop ?? "";
    }
}