////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Конфигурация клиента
/// </summary>
public class ClientConfigModel
{
    /// <summary>
    /// Описания элементов форм
    /// </summary>
    public required Dictionary<string, string> MetadataInput { get; set; }

    /// <summary>
    /// Описания страниц
    /// </summary>
    public required Dictionary<string, IEnumerable<string>> MetadataPage { get; set; }

    /// <summary>
    /// Информация об элементе формы
    /// </summary>
    public string? AboutInputInfo(string? input_name, string page_path, FormContextModel? form_context)
    {
        if (string.IsNullOrWhiteSpace(input_name))
            return null;
        IEnumerable<KeyValuePair<string, string>> _fd = MetadataInput.Where(x => x.Key.EndsWith(input_name, StringComparison.OrdinalIgnoreCase));
        if (!_fd.Any())
            return null;

        string _context_pref = form_context?.DataContext.FirstOrDefault(x => x.Key.Equals("pty", StringComparison.OrdinalIgnoreCase)).Value ?? page_path;

        if (string.IsNullOrWhiteSpace(_context_pref) || _context_pref.Equals("255")) // 255 - это тип порта NC (=не настроено)
            return MetadataInput.FirstOrDefault(x => x.Key.Equals(input_name, StringComparison.OrdinalIgnoreCase)).Value;

        return
            MetadataInput.FirstOrDefault(x => x.Key.Equals($"{_context_pref}#{input_name}", StringComparison.OrdinalIgnoreCase)).Value
            ?? MetadataInput.FirstOrDefault(x => x.Key.Equals(input_name, StringComparison.OrdinalIgnoreCase)).Value;

    }
}