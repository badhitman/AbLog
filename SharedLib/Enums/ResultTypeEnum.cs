﻿namespace SharedLib
{
    /// <summary>
    /// Типы сообщения (результата-ответа):
    /// Ошибка, Успех, Информация, Оповещение, Предупреждение
    /// </summary>
    public enum ResultTypeEnum
    {
        /// <summary>
        /// Ошибка
        /// </summary>
        Error = -1,

        /// <summary>
        /// Успех
        /// </summary>
        Success = 0,

        /// <summary>
        /// Информация
        /// </summary>
        Info = 2,

        /// <summary>
        /// Оповещение
        /// </summary>
        Alert = 3,

        /// <summary>
        /// Предупреждение
        /// </summary>
        Warning = 4
    }
}