////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Условия/Ограничения, зависящее от заданного значения/состояния заданного порта. Ответ rest/api
/// </summary>
public class ConditionsAnonimResponseModel : ResponseBaseModel
{
    /// <summary>
    /// Условия/Ограничения, зависящее от заданного значения/состояния заданного порта
    /// </summary>
    public ConditionAnonimModel[]? Conditions { get; set; }
}