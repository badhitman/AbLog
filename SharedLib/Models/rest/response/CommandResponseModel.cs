namespace SharedLib
{
    /// <summary>
    /// Команда скрипта. ответ на запрос rest/api
    /// </summary>
    public class CommandResponseModel : ResponseBaseModel
    {
        /// <summary>
        /// Команда скрипта
        /// </summary>
        public CommandModelDB? Command { get; set; }
    }
}