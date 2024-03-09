////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Сообщение результата выполнения запроса rest/api
/// </summary>
public class ResultMessage
{
    /// <summary>
    /// Тип сообщения: Ошибка, Успех, Информация, Оповещение, Предупреждение
    /// </summary>
    public ResultTypesEnum TypeMessage { get; set; }

    /// <summary>
    /// Текст сообщения
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <inheritdoc/>
    public override string ToString() => $"({TypeMessage}) {Text}";
}