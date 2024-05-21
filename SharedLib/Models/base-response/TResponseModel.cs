////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Базовая модель ответа/результата на запрос
/// </summary>
public class TResponseModel<T> : ResponseBaseModel
{
    /// <inheritdoc/>
    public TResponseModel() { }

    /// <inheritdoc/>
    public TResponseModel(List<ResultMessage> messages) { Messages = [.. messages]; }

    /// <inheritdoc/>
    public T? Response { get; set; }
}