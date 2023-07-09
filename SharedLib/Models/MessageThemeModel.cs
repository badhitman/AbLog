////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Сообщение
/// </summary>
public class MessageThemeModel : ThemeModel
{
    /// <summary>
    /// идентификатор
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// текст
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Дата/вфремя создания
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// Сообщение
    /// </summary>
    /// <param name="css_theme">цветовая тема</param>
    /// <param name="id">идентификатор</param>
    /// <param name="text_message">текст сообщения</param>
    public MessageThemeModel(string id, string text_message, ThemeModel css_theme) : base(css_theme.BgColor, css_theme.TextColor)
    {
        Id = id;
        Text = text_message;
    }
}