﻿////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// Команда скрипта
/// </summary>
public class CommandModelDB : EntrySortingModel
{
    /// <summary>
    /// Тип команды (команда контролера, порт или выход/переход)
    /// </summary>
    public TypesCommandsEnum CommandType { get; set; }

    /// <summary>
    /// Ограничения (условия запрета) выполнения команды
    /// </summary>
    public List<CommandConditionModelDB>? Conditions { get; set; }

    /// <summary>
    /// Родительский скрипт-владелец
    /// </summary>
    [JsonIgnore]
    public ScriptModelDB? Script { get; set; }
    /// <summary>
    /// FK: Родительский скрипт-владелец
    /// </summary>
    public int ScriptId { get; set; }

    /// <summary>
    /// Пауза (в секундах) перед выполнением команды. Пауза выдерживается средствами самого сервера, а не управляющего блока.
    /// Пауза выдерживается до проверки условия выполнения, т.е. не зависит от того: будет ли команда выполняться или нет
    /// </summary>
    [Range(1, 86400)]
    public int PauseSecondsBeforeExecution { get; set; }

    /// <summary>
    /// Скрытая команда. Т.е. она не будет выполняться в порядке общей очереди.
    /// На такую команду можно переместиться только принудительно из другой команды.
    /// После отработки команды дальнейшая очередь будет продолжена как обычно.
    /// </summary>
    public bool Hidden { get; set; }

    /// <summary>
    /// Идентификатор мастер-объекта. В зависимости от типа команды - Укзывает на один из трёх типов данных:
    /// Controller: означает, что это идентификатор устройства. В этом случае контроллер должен будет выполнить низкоуровневую команду. Сама команда для исполнения будет извлечена из поля "ExecutionParametr".
    /// PortImap: означает, что это идентификатор порта одного из устройств. Команда для исполнения (off/on) будет извлечена из поля "ExecutionParametr"
    /// Exit: означает, что текущий скрипт/сценарий прекращает своё выполнение. Дальнейшее поведение зависит от хранимого значения "Execution". Если "0" или null или string.Empty, то выход и окончание работы. Если значение десятичное и больше нуля, то это указатель на другую команду в другом (или этом же) скрипте с которого и должно продолжится выполнение. В таких случаях можно перейти в том числе на 'скрытую команду'.
    /// </summary>
    public int Execution { get; set; }
    /// <summary>
    /// Параметр для выполнения команды. В зависимости от типа команды укзывает:
    /// Controller: (string) низкоуровневая команда для отправки управляющему блоку. Например: 10:0;8:1;12:0
    /// PortImap: (string. возможные/валидные значения ['on'|'off']) признак, указывающий на новое/требуемое состояние порта. Если не 'on' и не 'off' (не зависимо от регистра), то порт будет переключён в противоположное состояние
    /// Exit: (int) указание на идентификатор команды, с которой следует начать выполнять скрипт, в который передаётся дальнейшее управление. Если &lt;= 0, то выход из выполнения скрипта
    /// </summary>
    public string? ExecutionParametr { get; set; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{Name}{(Hidden ? " [H]" : "")}{(PauseSecondsBeforeExecution > 0 ? $" [p {PauseSecondsBeforeExecution} sec]" : "")}";
    }
}