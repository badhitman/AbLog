using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorWasmApp.UIInterfaces
{
    /// <summary>
    /// Интерфейс вкладки
    /// </summary>
    public interface ITab
    {
        /// <summary>
        /// Содержимое вкладки
        /// </summary>
        RenderFragment? ChildContent { get; }
    }
}
