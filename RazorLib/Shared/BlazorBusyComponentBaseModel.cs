using Microsoft.AspNetCore.Components;

namespace RazorLib
{
    /// <summary>
    /// Базовый компонент с поддержкой состояния "занят". Компоненты, которые выполняют запросы
    /// на время обработки переходят в состояние "IsBusyProgress" с целью обеспечения визуализации смены этого изменения
    /// </summary>
    public abstract class BlazorBusyComponentBaseModel : ComponentBase
    {
        /// <summary>
        /// Компонент занят отправкой REST запроса и обработки ответа
        /// </summary>
        public bool IsBusyProgress { get; protected set; } = false;

        /// <summary>
        /// Текущий стиль кнопки в зависимости от состояния: "занят" или "готов"
        /// </summary>
        public string ButtonStyle => $"padding:0;margin:0;text-decoration:{(IsBusyProgress ? "none" : "underline")};cursor:{(IsBusyProgress ? "none" : "pointer")};";
    }
}
