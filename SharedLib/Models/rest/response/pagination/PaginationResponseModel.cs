////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Newtonsoft.Json;

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



    /// <summary>
    /// Результат обработки запроса.
    /// True - если удачно бз ошибок. False  - если возникли ошибки
    /// </summary>
    [JsonIgnore]
    public virtual bool IsSuccess => !Messages.Any(x => x.TypeMessage == ResultTypeEnum.Error);

    /// <summary>
    /// Сообщение сервера. Если IsSuccess == false, то будет сообщение об ошибке
    /// </summary>
    [JsonIgnore]
    public string Message
    {
        get
        {
            Messages ??= new List<ResultMessage>();
            return Messages.Any() ? $"{string.Join($",{Environment.NewLine}", Messages.Select(x => $"[{x}]"))};" : string.Empty;
        }
    }



    /// <summary>
    /// Создать ответ с ошибкой
    /// </summary>
    public static PaginationResponseModel CreateError(string msg) => new() { Messages = new List<ResultMessage>() { new ResultMessage() { TypeMessage = ResultTypeEnum.Error, Text = msg } } };

    /// <summary>
    /// Создать ответ с Success
    /// </summary>
    public static PaginationResponseModel CreateSuccess(string msg) => new() { Messages = new List<ResultMessage>() { new ResultMessage() { TypeMessage = ResultTypeEnum.Success, Text = msg } } };

    /// <summary>
    /// Создать ответ с Warning
    /// </summary>
    public static PaginationResponseModel CreateWarning(string msg) => new() { Messages = new List<ResultMessage>() { new ResultMessage() { TypeMessage = ResultTypeEnum.Warning, Text = msg } } };

    /// <summary>
    /// Добавить сообщение: Успех
    /// </summary>
    public PaginationResponseModel AddSuccess(string text) => AddMessage(ResultTypeEnum.Success, text);

    /// <summary>
    /// Добавить сообщения: Успех
    /// </summary>
    public PaginationResponseModel AddRangeSuccess(IEnumerable<string> messages)
    {
        foreach (string text in messages)
            AddMessage(ResultTypeEnum.Success, text);
        return this;
    }

    /// <summary>
    /// Добавить сообщение: Информация
    /// </summary>
    public PaginationResponseModel AddInfo(string text) => AddMessage(ResultTypeEnum.Info, text);

    /// <summary>
    /// Добавить сообщение: Ошибка
    /// </summary>
    public PaginationResponseModel AddError(string text) => AddMessage(ResultTypeEnum.Error, text);

    /// <summary>
    /// Добавить сообщение: Оповещение
    /// </summary>
    public PaginationResponseModel AddAlert(string text) => AddMessage(ResultTypeEnum.Alert, text);

    /// <summary>
    /// Добавить сообщение: Внимание
    /// </summary>
    public PaginationResponseModel AddWarning(string text) => AddMessage(ResultTypeEnum.Warning, text);

    /// <summary>
    /// Сообщения к ответу rest/api
    /// </summary>
    public List<ResultMessage> Messages { get; set; } = new();

    /// <summary>
    /// Добавить сообщение к результату-ответу rest/api
    /// </summary>
    public PaginationResponseModel AddMessage(ResultTypeEnum type, string text)
    {
        Messages ??= new();
        Messages.Add(new ResultMessage() { TypeMessage = type, Text = text });
        return this;
    }

    /// <summary>
    /// Добавить сообщения
    /// </summary>
    public void AddMessages(IEnumerable<ResultMessage> messages) => Messages.AddRange(messages);
}