////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib
{
    /// <summary>
    /// Лог
    /// </summary>
    public class LogRowModelDB : EntryModel
    {
        /// <summary>
        /// Дата/время создания
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// Статус лога:
        /// ошибка, трассировка, отладка, информация, предупреждение
        /// </summary>
        public LogStatusesEnum Status { get; set; }

        /// <summary>
        /// маркер-метка строки логов
        /// </summary>
        public string TAG { get; set; } = "null";

        /// <inheritdoc/>
        public override string ToString()
        {
            string ret_val = $"[{CreatedAt}]|[{Status}]|[{TAG}] {Name}";
            return ret_val;
        }
    }
}