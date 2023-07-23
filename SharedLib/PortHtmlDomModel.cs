namespace SharedLib;

/// <summary>
/// Порт устройства
/// </summary>
public class PortHtmlDomModel : HtmlDomModel
{
    /// <summary>
    /// Получить данные по порту
    /// </summary>
    public string TelegramResponseHtmlRaw()
    {
        if (!this.Any())
            return "данные отсутсвуют. error {9EDCD529-B1F4-45F5-9405-60AA5C2FB30B}";

        string res = "";
        foreach (TreeItemDataModel item in this)
            ReadNode(ref res, item);

        return res;
    }

    void ReadNode(ref string tg_resp_html, TreeItemDataModel item, int deep_num = 0)
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
            case "form":
                if (item.TreeItems?.Any() == true)
                {
                    deep_num++;
                    foreach (TreeItemDataModel sub_item in item.TreeItems)
                        ReadNode(ref tg_resp_html, sub_item, deep_num);
                }
                break;
            case "input":
                string? input_type = item.Attributes?.FirstOrDefault(x => x.Key.Equals("type", StringComparison.OrdinalIgnoreCase)).Value;
                if (new[] { "submit", "hidden" }.Any(x => x.Equals(input_type, StringComparison.OrdinalIgnoreCase)))
                    break;

                if (input_type?.Equals("checkbox", StringComparison.OrdinalIgnoreCase) == true)
                {
                    bool is_checked = item.Attributes?.Any(x => x.Key.Equals("checked", StringComparison.OrdinalIgnoreCase)) == true;
                    tg_resp_html += $"{_pref}{item.Text ?? $"{(is_checked ? "✅" : "▢")} <b>{item.Attributes?.FirstOrDefault(x => x.Key.Equals("name", StringComparison.OrdinalIgnoreCase)).Value}</b> <i>({item.Attributes?.FirstOrDefault(x => x.Key.Equals("value", StringComparison.OrdinalIgnoreCase)).Value})</i>"}\n";
                    //if (item.Attributes?.Any() == true)
                    //    tg_resp_html = $"{tg_resp_html.TrimEnd()} {$"({string.Join(",", item.Attributes.Select(x => $"[{x.Key}:'{x.Value}']"))})"}\n";
                }
                else
                {
                    tg_resp_html += $"{_pref}🔠 {item.Text ?? $""}\n";
                    if (item.Attributes?.Any() == true)
                        tg_resp_html = $"{tg_resp_html.TrimEnd()} {$"({string.Join(",", item.Attributes.Select(x => $"[{x.Key}:'{x.Value}']"))})"}\n";
                }

                break;
            //case "#text":
            //    tg_resp_html += $"{_pref}{item.Text}";                
            //    break;
            default:
                tg_resp_html += $"{_pref}{item.Text ?? $"-{item.NodeName}-"}\n";
                if (item.Attributes?.Any() == true)
                    tg_resp_html = $"{tg_resp_html.TrimEnd()} {$"({string.Join(",", item.Attributes.Select(x => $"[{x.Key}:'{x.Value}']"))})"}\n";

                if (item.TreeItems?.Any() == true)
                {
                    deep_num++;
                    foreach (TreeItemDataModel sub_item in item.TreeItems)
                        ReadNode(ref tg_resp_html, sub_item, deep_num);
                }
                break;
        }
    }

    static void ReadSelectNode(ref string tg_resp_html, TreeItemDataModel item, string _pref)
    {
        //tg_resp_html += $"{_pref}{item.Text ?? $"-{item.NodeName}-"}\n";
        //if (item.Attributes?.Any() == true)
        //    tg_resp_html = $"{tg_resp_html.TrimEnd()} {$"({string.Join(",", item.Attributes.Select(x => $"[{x.Key}:'{x.Value}']"))})"}";

        string selected_val = GetSelectedItem(item.TreeItems);

        if (!string.IsNullOrEmpty(selected_val))
            tg_resp_html = $"{tg_resp_html.TrimEnd()}: <u>{item.Attributes?.FirstOrDefault(x => x.Key.Equals("name", StringComparison.OrdinalIgnoreCase)).Value}</u> 📚 {selected_val}\n";
    }

    static string GetSelectedItem(HtmlDomModel? treeItems)
    {
        if (treeItems?.Any() != true)
            return "";

        string? value_prop = null; ;
        foreach (TreeItemDataModel item in treeItems)
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