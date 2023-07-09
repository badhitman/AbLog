////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Базовая модель ответа с поддержкой пагинации
/// </summary>
public class PaginationResponseModel : PaginationRequestModel
{
    /// <summary>
    /// Общее/всего количество элементов
    /// </summary>
    public int TotalRowsCount { get; set; }

    /// <summary>
    /// Количесвто страниц пагинатора
    /// </summary>
    /// <param name="page_size"></param>
    /// <param name="total_rows_count"></param>
    /// <param name="default_page_size"></param>
    /// <returns></returns>
    public static uint CalcTotalPagesCount(int page_size, int total_rows_count, uint default_page_size = 10)
    {
        if (page_size == 0)
            return (uint)Math.Ceiling((double)total_rows_count / (double)default_page_size);

        return (uint)Math.Ceiling((double)total_rows_count / (double)page_size);
    }

    /// <summary>
    /// Базовая модель ответа с поддержкой пагинации
    /// </summary>
    public PaginationResponseModel() { }

    /// <summary>
    /// Базовая модель ответа с поддержкой пагинации
    /// </summary>
    /// <param name="init_object">Объект инициализации пагинатора</param>
    public PaginationResponseModel(PaginationRequestModel init_object) : base(init_object) { }
}