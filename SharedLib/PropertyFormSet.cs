////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Поле формы для заполнения
/// </summary>
/// <param name="Code">Код поля формы</param>
/// <param name="Title">Название поля для пользователя</param>
/// <param name="AllowedValues">Доступные значения (по желанию). Если в этом перечне есть элементы, то значения в это поле будут ограничены этим списком</param>
public record struct PropertyFormSet(string Code, string Title, string[]? AllowedValues = null);