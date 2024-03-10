////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Сообщение
/// </summary>
/// <param name="css_theme">цветовая тема</param>
/// <param name="id">идентификатор</param>
/// <param name="text_message">текст сообщения</param>
public class MessageThemeModel(string id, string text_message, ThemeModel css_theme)
    : ThemeModel(css_theme.BgColor, css_theme.TextColor)
{
    /// <summary>
    /// идентификатор
    /// </summary>
    public string Id { get; set; } = id;

    /// <summary>
    /// текст
    /// </summary>
    public string Text { get; set; } = text_message;

    /// <summary>
    /// Дата/вфремя создания
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}