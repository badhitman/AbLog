////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;

namespace RazorLib;

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
    /// Уведомляет компонент об изменении его состояния.
    /// Когда применимо, это вызовет повторную визуализацию компонента.
    /// </summary>
    public virtual void StateHasChangedCall() => StateHasChanged();
}