namespace SharedLib
{
    /// <summary>
    /// Условия/Ограничение, зависящее от заданного значения/состояния заданного порта. Ответ rest/api
    /// </summary>
    public class ConditionsAnonimResponseModel : ResponseBaseModel
    {
        /// <summary>
        /// Условия/Ограничение, зависящее от заданного значения/состояния заданного порта
        /// </summary>
        public ConditionAnonimModel[]? Conditions { get; set; }
    }
}