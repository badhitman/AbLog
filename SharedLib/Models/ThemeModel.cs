////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Тема (цветовая)
/// </summary>
public class ThemeModel
{
    /// <summary>
    /// цвет фона
    /// </summary>
    public string BgColor { get; set; }

    /// <summary>
    /// цвет текста
    /// </summary>
    public string TextColor { get; set; }

    /// <summary>
    /// Тема (цветовая)
    /// </summary>
    /// <param name="bg">фон</param>
    /// <param name="text">текст</param>
    public ThemeModel(string bg, string text)
    {
        BgColor = bg;
        TextColor = text;
    }
}