﻿////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Newtonsoft.Json;

namespace SharedLib;

/// <summary>
/// Базовая модель ответа сервера
/// </summary>
public class ResponseBaseModel
{
    /// <summary>
    /// Базовая модель ответа сервера
    /// </summary>
    public ResponseBaseModel() { }

    /// <summary>
    /// Базовая модель ответа сервера
    /// </summary>
    public ResponseBaseModel(string msg, ResultTypesEnum msgType = ResultTypesEnum.Success)
    {
        if (!string.IsNullOrWhiteSpace(msg))
        {
            AddMessage(msgType, msg);
        }
    }



    /// <summary>
    /// Результат обработки запроса.
    /// True - если удачно бз ошибок. False  - если возникли ошибки
    /// </summary>
    [JsonIgnore]
    public virtual bool IsSuccess => !Messages.Any(x => x.TypeMessage == ResultTypesEnum.Error);

    /// <summary>
    /// Сообщение сервера. Если IsSuccess == false, то будет сообщение об ошибке
    /// </summary>
    [JsonIgnore]
    public string Message
    {
        get
        {
            Messages ??= [];
            return Messages.Count != 0 ? $"{string.Join($",{Environment.NewLine}", Messages.Select(x => $"[{x}]"))};" : string.Empty;
        }
    }

    /// <summary>
    /// Создать ответ с ошибкой
    /// </summary>
    public static ResponseBaseModel CreateError(string msg) => new() { Messages = [new ResultMessage() { TypeMessage = ResultTypesEnum.Error, Text = msg }] };

    /// <summary>
    /// Создать ответ с Success
    /// </summary>
    public static ResponseBaseModel CreateSuccess(string msg) => new() { Messages = [new ResultMessage() { TypeMessage = ResultTypesEnum.Success, Text = msg }] };

    /// <summary>
    /// Создать ответ с Warning
    /// </summary>
    public static ResponseBaseModel CreateWarning(string msg) => new() { Messages = [new ResultMessage() { TypeMessage = ResultTypesEnum.Warning, Text = msg }] };

    /// <summary>
    /// Добавить сообщение: Успех
    /// </summary>
    public ResponseBaseModel AddSuccess(string text) => AddMessage(ResultTypesEnum.Success, text);

    /// <summary>
    /// Добавить сообщения: Успех
    /// </summary>
    public ResponseBaseModel AddRangeSuccess(IEnumerable<string> messages)
    {
        foreach (string text in messages)
            AddMessage(ResultTypesEnum.Success, text);
        return this;
    }

    /// <summary>
    /// Добавить сообщение: Информация
    /// </summary>
    public ResponseBaseModel AddInfo(string text) => AddMessage(ResultTypesEnum.Info, text);

    /// <summary>
    /// Добавить сообщение: Ошибка
    /// </summary>
    public ResponseBaseModel AddError(string text) => AddMessage(ResultTypesEnum.Error, text);

    /// <summary>
    /// Добавить сообщение: Оповещение
    /// </summary>
    public ResponseBaseModel AddAlert(string text) => AddMessage(ResultTypesEnum.Alert, text);

    /// <summary>
    /// Добавить сообщение: Внимание
    /// </summary>
    public ResponseBaseModel AddWarning(string text) => AddMessage(ResultTypesEnum.Warning, text);

    /// <summary>
    /// Сообщения к ответу rest/api
    /// </summary>
    public List<ResultMessage> Messages { get; set; } = [];

    /// <summary>
    /// Добавить сообщение к результату-ответу rest/api
    /// </summary>
    public ResponseBaseModel AddMessage(ResultTypesEnum type, string text)
    {
        Messages ??= [];
        Messages.Add(new ResultMessage() { TypeMessage = type, Text = text });
        return this;
    }

    /// <summary>
    /// Добавить сообщения
    /// </summary>
    public void AddMessages(IEnumerable<ResultMessage> messages) => Messages.AddRange(messages);



    /// <summary>
    /// Базовая модель ответа сервера rest/api
    /// </summary>
    public static implicit operator ResponseBaseModel(Exception ex)
    {
        ResponseBaseModel res = CreateError(System.Text.RegularExpressions.Regex.Replace(ex.Message, @"ORA-\d+:\s?", string.Empty));

        if (!string.IsNullOrWhiteSpace(ex.StackTrace))
        {
            res.AddError(ex.StackTrace);
        }

        Exception? ie = ex.InnerException;
        while (ie != null)
        {
            res.AddError(ie.Message);
            if (!string.IsNullOrWhiteSpace(ie.StackTrace))
            {
                res.AddError(ie.StackTrace);
            }
            ie = ie.InnerException;
        }
        return res;
    }
}