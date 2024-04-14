////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Базовая модель запроса с поддержкой пагинации
/// </summary>
public class PaginationRequestModel
{
    /// <inheritdoc/>
    public static PaginationRequestModel Build(PaginationRequestModel init)
    {
        return new PaginationRequestModel()
        {
            PageSize = init.PageSize,
            PageNum = init.PageNum,
            SortingDirection = init.SortingDirection,
            SortBy = init.SortBy
        };
    }

    /// <summary>
    /// Размер страницы (количество элементов на странице)
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Номер текущей страницы
    /// </summary>
    public int PageNum { get; set; }

    /// <summary>
    /// Сортировка (от большего к меньшему или от меньшего к большему)
    /// </summary>
    public VerticalDirectionsEnum SortingDirection { get; set; }

    /// <summary>
    /// Имя поля по которому должна происходить сортировка
    /// </summary>
    public string? SortBy { get; set; }
}