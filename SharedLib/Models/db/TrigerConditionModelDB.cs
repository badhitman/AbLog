////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedLib;

/// <summary>
/// Условие/повод запуска тригера
/// </summary>
[Index(nameof(OwnerId), nameof(HardwareId), nameof(PortId), IsUnique = true)]
public class TrigerConditionModelDB : ConditionBaseModelDB
{
    /// <summary>
    /// Условие/повод запуска тригера
    /// </summary>
    public TrigerConditionModelDB() { }

    /// <summary>
    /// Условие/повод запуска тригера
    /// </summary>
    /// <param name="condition_request"></param>
    public TrigerConditionModelDB(ConditionUpdateModel condition_request)
    {
        Id = condition_request.Id < 0 ? 0 : condition_request.Id;
        PortId = condition_request.PortId;
        HardwareId = condition_request.HardwareId;
        OwnerId = condition_request.OwnerId;
        Name = condition_request.Name;
        Value = condition_request.Value;
        ConditionValueType = condition_request.ConditionValueType;
        СomparisonMode = condition_request.СomparisonMode;
    }

    /// <summary>
    /// Тригер запуска скрипта
    /// </summary>
    [JsonIgnore, ForeignKey(nameof(OwnerId))]
    public TrigerModelDB? Triger { get; set; }
}