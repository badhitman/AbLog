////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Входящий запрос от управляющего блока
/// </summary>
public class TaskHttpHandlerModel : TaskModelDB
{
    /// <summary>
    /// Дата/время запроса
    /// </summary>
    public DateTime InitDateTime { get; set; } = DateTime.Now;

    /// <summary>
    /// Имя (megad id) управляющего блока
    /// </summary>
    public string? MegadID { get; set; }

    /// <summary>
    /// Порт исполнительного блока
    /// </summary>
    public string? MegadPort { get; set; }

    /// <summary>
    /// Значение порта
    /// </summary>
    public string? Value { get; set; }

    /// <summary>
    /// Значение порта (ver.2)
    /// </summary>
    public string? Value2 { get; set; }

    /// <summary>
    /// Исполнительный блок при включении оповещает сервер
    /// </summary>
    public bool ControllerIsPowerOn { get; set; }

    /// <summary>
    /// Счётчик срабатываний порта
    /// </summary>
    public int PortTargetsCount { get; set; }

    /// <summary>
    /// Направление изменения значения порта (понижение/повышение)
    /// </summary>
    public VerticalDirectionsEnum? TransitionDirectionValue { get; set; }

    /// <summary>
    /// Обработка длительного нажатия (удержания) кнопки
    /// </summary>
    public ButtonClickDurationsBehaviorsMomentsEnum? ButtonClickDurationBehaviorMoment { get; set; }

    /// <summary>
    /// Обработка двойных нажатий на кнопку
    /// </summary>
    public ButtonClickModesEnum? ButtonClickMode { get; set; }

    /// <summary>
    /// Полный URI запроса
    /// </summary>
    public string? RequestUri { get; private set; }
}