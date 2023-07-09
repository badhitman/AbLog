////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
    public TrigerConditionModelDB(UpdateConditionRequestModel condition_request)
    {
        this.Id = condition_request.Id < 0 ? 0 : condition_request.Id;
        this.PortId = condition_request.PortId;
        this.HardwareId = condition_request.HardwareId;
        this.OwnerId = condition_request.OwnerId;
        this.Name = condition_request.Name;
        this.Value = condition_request.Value;
        this.ConditionValueType = condition_request.ConditionValueType;
        this.СomparisonMode = condition_request.СomparisonMode;
    }

    /// <summary>
    /// Тригер запуска скрипта
    /// </summary>
    [JsonIgnore, ForeignKey(nameof(OwnerId))]
    public TrigerModelDB? Triger { get; set; }
}