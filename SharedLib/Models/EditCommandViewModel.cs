////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Редактирование команды (view модель)
/// </summary>
public class EditCommandViewModel : EntryModel
{
    /// <summary>
    /// Пауза перед выполнением
    /// </summary>
    public int PauseSecondsBeforeExecution { get; set; }

    /// <summary>
    /// Скрытая команда?
    /// Скрытая команда не наступает по ходу простой очерёдности команд.
    /// На такую команду можно перейти только целевым указателем перехода.
    /// </summary>
    public bool Hidden { get; set; }

    /// <summary>
    /// Тип команды
    /// </summary>
    public TypesCommandsEnum CommandType { get; set; }

    /// <summary>
    /// Идентификатор обслуживающей сущности выполнения команды.
    /// В зависимости от типа команды указывает:
    /// Controller: означает, что это идентификатор устройства. Команду выполнять управляющему блоку. Сама команда для исполнения будет извлечена из поля "ExecutionParameter".
    /// PortImap: означает, что это идентификатор порта некоторого устройства. Команда для исполнения (off/on) будет извлечена из поля "ExecutionParameter"
    /// Exit: означает, что текущий скрипт/сценарий прекращает своё выполнение. Дальнейшее поведение зависит от хранимого значения "Execution". Если "0" или null или string.Empty, то выход и окончание работы. Если значение десятичное и больше нуля, то это указатель на другой скрипт, который следует выполнить.
    /// </summary>
    public int Execution { get; set; }
    /// <summary>
    /// Параметр для выполнения команды.
    /// В зависимости от типа команды указывает:
    /// Controller: [string] низкоуровневая команда для отправки управляющему блоку. Например: 10:0;8:1;12:0
    /// PortImap: [on/off] признак, указывающий на новое(требуемое) состояние порта
    /// Exit: [int] указание на идентификатор команды, с которой следует начать выполнять скрипт, в который передаётся дальнейшее управление
    /// </summary>
    public string? ExecutionParameter { get; set; }

    /// <summary>
    /// Приведение/преобразование типов
    /// </summary>
    public static implicit operator CommandModelDB(EditCommandViewModel val)
    {
        return new CommandModelDB()
        {
            Name = val.Name,
            PauseSecondsBeforeExecution = val.PauseSecondsBeforeExecution,
            Hidden = val.Hidden,
            CommandType = val.CommandType,
            Execution = val.Execution,
            ExecutionParameter = val.ExecutionParameter,
            Id = val.Id,
        };
    }

    /// <summary>
    /// Приведение/преобразование типов
    /// </summary>
    public static implicit operator EditCommandViewModel(CommandModelDB val)
    {
        return new EditCommandViewModel()
        {
            Name = val.Name,
            PauseSecondsBeforeExecution = val.PauseSecondsBeforeExecution,
            Hidden = val.Hidden,
            CommandType = val.CommandType,
            Execution = val.Execution,
            ExecutionParameter = val.ExecutionParameter,
            Id = val.Id
        };
    }

    /// <summary>
    /// Перегрузка сравнения типов
    /// </summary>
    public static bool operator ==(EditCommandViewModel com1, CommandModelDB com2)
    {
        return
            com1.Hidden == com2.Hidden &&
            com1.ExecutionParameter == com2.ExecutionParameter &&
            com1.Execution == com2.Execution &&
            com1.PauseSecondsBeforeExecution == com2.PauseSecondsBeforeExecution &&
            com1.CommandType == com2.CommandType &&
            com1.Name == com2.Name;
    }

    /// <summary>
    /// Перегрузка сравнения типов
    /// </summary>
    public static bool operator !=(EditCommandViewModel com1, CommandModelDB com2)
    {
        return !(com1 == com2);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return this == obj;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}