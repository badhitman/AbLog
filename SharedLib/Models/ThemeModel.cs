////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Тема (цветовая)
/// </summary>
public class ThemeModel(string bg, string text)
{
    /// <summary>
    /// цвет фона
    /// </summary>
    public string BgColor { get; set; } = bg;

    /// <summary>
    /// цвет текста
    /// </summary>
    public string TextColor { get; set; } = text;
}