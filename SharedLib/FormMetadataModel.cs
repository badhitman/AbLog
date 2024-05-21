////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Форма
/// </summary>
/// <param name="Title">Заголовок формы</param>
/// <param name="FormProperties">Поля формы</param>
public record struct FormMetadataModel(string Title, PropertyFormSet[] FormProperties);
