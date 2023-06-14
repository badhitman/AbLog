using Microsoft.AspNetCore.Components;

namespace BlazorWasmApp.UIInterfaces
{
    /// <summary>
    /// Интерфейс вкладки
    /// </summary>
    public interface INav
    {
        /// <summary>
        /// Содержимое вкладки
        /// </summary>
        RenderFragment? ChildContent { get; }
    }
}