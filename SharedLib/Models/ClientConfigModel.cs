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
    public string? AboutInputInfo(string? input_name, string page_path)
    {
        if (string.IsNullOrWhiteSpace(input_name))
            return null;
        IEnumerable<KeyValuePair<string, string>> _fd = MetadataInput.Where(x => x.Key.EndsWith(input_name, StringComparison.OrdinalIgnoreCase));
        if (!_fd.Any())
            return null;

        if (string.IsNullOrWhiteSpace(page_path))
            return MetadataInput.FirstOrDefault(x => x.Key.Equals(input_name, StringComparison.OrdinalIgnoreCase)).Value;

        return
            MetadataInput.FirstOrDefault(x => x.Key.Equals($"{page_path}#{input_name}", StringComparison.OrdinalIgnoreCase)).Value
            ?? MetadataInput.FirstOrDefault(x => x.Key.Equals(input_name, StringComparison.OrdinalIgnoreCase)).Value;

    }
}
