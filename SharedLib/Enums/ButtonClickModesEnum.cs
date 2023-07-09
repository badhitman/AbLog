////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Режимы обработчика нажатия кнопки: (обработка одинарных и двойных кликов/нажатий): 1 или 2 - в зависимости каким образом сработала кнопка
/// string click = uri.GetQueryParameter("click");
/// </summary>
public enum ButtonClickModesEnum
{
    /// <summary>
    /// Одинарный клик кнопки
    /// </summary>
    SingleClick = 1,

    /// <summary>
    /// Двойной клик кнопки
    /// </summary>
    DoubleClick = 2,
}