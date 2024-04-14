////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedLib;

/// <summary>
/// Запрещающее условие (ограничение) для выполнения команды.
/// </summary>
[Index(nameof(OwnerId), nameof(HardwareId), nameof(PortId), IsUnique = true)]
public class CommandConditionModelDB : ConditionBaseModelDB
{
    /// <inheritdoc/>
    public static CommandConditionModelDB Build(ConditionUpdateModel condition_request)
    {
        return new CommandConditionModelDB()
        {
            Id = condition_request.Id,
            PortId = condition_request.PortId,
            HardwareId = condition_request.HardwareId,
            OwnerId = condition_request.OwnerId,
            Name = condition_request.Name,
            Value = condition_request.Value,
            ConditionValueType = condition_request.ConditionValueType,
            СomparisonMode = condition_request.СomparisonMode
        };
    }

    /// <summary>
    /// Команда-владелец
    /// </summary>
    [JsonIgnore, ForeignKey(nameof(OwnerId))]
    public CommandModelDB? Command { get; set; }
}