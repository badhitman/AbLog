////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib
{
    /// <summary>
    /// Статусы лог-сообщений:
    /// ошибка, трассировка, отладка, информация, предупреждение
    /// </summary>
    public enum LogStatusesEnum
    {
        /// <summary>
        /// Ошибка
        /// </summary>
        Error = -10,

        /// <summary>
        /// Трассировка
        /// </summary>
        Trac = 0,

        /// <summary>
        /// Отладка
        /// </summary>
        Debug = 10,

        /// <summary>
        /// Информация
        /// </summary>
        Info = 20,

        /// <summary>
        /// Важное
        /// </summary>
        Warn = 30
    }
}