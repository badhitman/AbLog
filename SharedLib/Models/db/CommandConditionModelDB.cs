////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace SharedLib;

/// <summary>
/// Запрещающее условие (ограничение) для выполнения команды.
/// </summary>
[Index(nameof(OwnerId), nameof(HardwareId), nameof(PortId), IsUnique = true)]
public class CommandConditionModelDB : ConditionBaseModelDB
{
    /// <summary>
    /// Запрещающее условие (ограничение) для выполнения команды.
    /// </summary>
    public CommandConditionModelDB() { }

    /// <summary>
    /// Запрещающее условие (ограничение) для выполнения команды.
    /// </summary>
    public CommandConditionModelDB(ConditionUpdateModel condition_request)
    {
        this.Id = condition_request.Id;
        this.PortId = condition_request.PortId;
        this.HardwareId = condition_request.HardwareId;
        this.OwnerId = condition_request.OwnerId;
        this.Name = condition_request.Name;
        this.Value = condition_request.Value;
        this.ConditionValueType = condition_request.ConditionValueType;
        this.СomparisonMode = condition_request.СomparisonMode;
    }

    /// <summary>
    /// Команда-владелец
    /// </summary>
    [JsonIgnore, ForeignKey(nameof(OwnerId))]
    public CommandModelDB? Command { get; set; }
}