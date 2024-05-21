////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Признак поведения в данный момент.
/// (обработка длительных удержаний кнопок):
/// 1 - После отпускания клавиши после длительного нажатия,
/// 2 - при длительном удержании кнопки
/// ~ string m = uri.GetQueryParameter("m");
/// </summary>
public enum ButtonClickDurationsBehaviorsMomentsEnum
{
    /// <summary>
    /// 1 - После отпускания клавиши после длительного нажатия
    /// </summary>
    AfterReleasingButton = 1,

    /// <summary>
    /// 2 - при длительном удержании кнопки
    /// </summary>
    ByHoldingButtonLongTime = 2
}