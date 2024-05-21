////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedLib;

/// <summary>
/// Условие/повод запуска триггера
/// </summary>
[Index(nameof(OwnerId), nameof(HardwareId), nameof(PortId), IsUnique = true)]
public class TrigerConditionModelDB : ConditionBaseModelDB
{
    /// <inheritdoc/>
    public static TrigerConditionModelDB Build(ConditionUpdateModel condition_request)
    {
        return new TrigerConditionModelDB()
        {
            Id = condition_request.Id < 0 ? 0 : condition_request.Id,
            PortId = condition_request.PortId,
            HardwareId = condition_request.HardwareId,
            OwnerId = condition_request.OwnerId,
            Name = condition_request.Name,
            Value = condition_request.Value,
            ConditionValueType = condition_request.ConditionValueType,
            ComparisonMode = condition_request.ComparisonMode
        };
    }

    /// <summary>
    /// Триггер запуска скрипта
    /// </summary>
    [JsonIgnore, ForeignKey(nameof(OwnerId))]
    public TrigerModelDB? Triger { get; set; }
}