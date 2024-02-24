////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using ab.context;
using Microsoft.AspNetCore.Mvc;
using SharedLib;

namespace ABLog4;

/// <summary>
/// 
/// </summary>
[ApiController]
[Route("/api/[controller]")]
public class ConditionsController : ControllerBase
{
    private readonly ILogger<ConditionsController> _logger;

    /// <summary>
    /// 
    /// </summary>
    public ConditionsController(ILogger<ConditionsController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// 
    /// </summary>
    [HttpGet("{owner_id}/{condition_type}")]
    public ConditionsAnonimResponseModel ConditionsGetByOwner(int owner_id, ConditionsTypesEnum condition_type)
    {
        ConditionsAnonimResponseModel res_ac = new();
        lock (ServerContext.DbLocker)
        {
            using ServerContext db = new();
            res_ac.Conditions = condition_type switch
            {
                ConditionsTypesEnum.Command => db.ConditionsCommands.Where(x => x.OwnerId == owner_id).Select(x => new ConditionAnonimModel() { Id = x.Id, Name = x.Name, Value = x.Value, ÑomparisonMode = x.ÑomparisonMode, ConditionValueType = x.ConditionValueType, HardwareId = x.HardwareId, PortId = x.PortId }).ToArray(),
                ConditionsTypesEnum.Trigger => db.TrigersConditions.Where(x => x.OwnerId == owner_id).Select(x => new ConditionAnonimModel() { Id = x.Id, Name = x.Name, Value = x.Value, ÑomparisonMode = x.ÑomparisonMode, ConditionValueType = x.ConditionValueType, HardwareId = x.HardwareId, PortId = x.PortId }).ToArray(),
                _ => throw new NotImplementedException()
            };
        }
        return res_ac;
    }

    /// <summary>
    /// 
    /// </summary>
    [HttpPost($"/{GlobalStatic.Routes.UPDATE}")]
    public ConditionsAnonimResponseModel ConditionUpdateOrCreate(UpdateConditionRequestModel condition_request)
    {
        ConditionsAnonimResponseModel res_ac = new();
        if (condition_request is null)
        {
            res_ac.AddError("Îøèáêà âûïîëíåíèÿ çàïðîñà: {A6A02232-9F35-4BA3-AF50-D8767A5C2FC9}");
            return res_ac;
        }
        ResponseBaseModel check = condition_request.ValidateCondition;
        if (!check.IsSuccess)
            return (ConditionsAnonimResponseModel)check;

        if (string.IsNullOrWhiteSpace(condition_request.Name))
        {
            res_ac.AddWarning($"Äëÿ ýëåìåíòà{(condition_request.Id > 0 ? $" #{condition_request.Id}" : "")} íå óêàçàíî èìÿ (");
        }

        if (condition_request.ConditionValueType == ÑomparisonsValuesTypesEnum.ValueAsDecimal && (condition_request.ÑomparisonMode == ÑomparisonsModesEnum.Equal || condition_request.ÑomparisonMode == ÑomparisonsModesEnum.NotEqual))
        {
            res_ac.AddInfo($"Äëÿ ýëåìåíòà{(condition_request.Id > 0 ? $" #{condition_request.Id}" : "")} óñòàíîâëåíî ñòðîãîå ÷èñëîâîå ñîîòâåòñâèå. Ïîïàäàíèå â òàêîå óñëîâèå ìàëîâåðîÿòíî!");
        }

        lock (ServerContext.DbLocker)
        {
            using ServerContext db = new();

            switch (condition_request.ConditionType)
            {
                case ConditionsTypesEnum.Command:
                    if (db.ConditionsCommands.Any(x => x.HardwareId == condition_request.HardwareId && x.PortId == condition_request.PortId && x.OwnerId == condition_request.OwnerId && x.Id != condition_request.Id))
                    {
                        res_ac.AddError($"Ýòîò êîíòðîëëåð/ïîðò óæå èñïîëüçóþòñÿ");
                        return res_ac;
                    }
                    break;
                case ConditionsTypesEnum.Trigger:
                    if (db.TrigersConditions.Any(x => x.HardwareId == condition_request.HardwareId && x.PortId == condition_request.PortId && x.OwnerId == condition_request.OwnerId && x.Id != condition_request.Id))
                    {
                        res_ac.AddError($"Ýòîò êîíòðîëëåð/ïîðò óæå èñïîëüçóþòñÿ");
                        return res_ac;
                    }
                    break;
                default:
                    throw new NotImplementedException("{1327DA2E-0452-4831-8C22-FEF6186BCF9D}");
            }

            if (condition_request.Id > 0)
            {
                switch (condition_request.ConditionType)
                {
                    case ConditionsTypesEnum.Command:
                        db.Update(new CommandConditionModelDB(condition_request));
                        break;
                    case ConditionsTypesEnum.Trigger:
                        TrigerConditionModelDB new_t = new(condition_request);
                        db.Update(new_t);
                        break;
                    default:
                        throw new NotImplementedException("{1327DA2E-0452-4831-8C22-FEF6186BCF9D}");
                }
                res_ac.AddSuccess($"Ýëåìåíò #{condition_request.Id} îáíîâë¸í");
            }
            else
            {
                switch (condition_request.ConditionType)
                {
                    case ConditionsTypesEnum.Command:
                        db.Add(new CommandConditionModelDB(condition_request));
                        break;
                    case ConditionsTypesEnum.Trigger:
                        db.Add(new TrigerConditionModelDB(condition_request));
                        break;
                    default:
                        throw new NotImplementedException("{406FEE2B-BD49-44D6-9AB3-78902227ED9E}");
                }
                res_ac.AddSuccess($"Ýëåìåíò ñîçäàí");
            }
            db.SaveChanges();

            res_ac.Conditions = condition_request.ConditionType switch
            {
                ConditionsTypesEnum.Command => db.ConditionsCommands.Where(x => x.OwnerId == condition_request.OwnerId).Select(x => new ConditionAnonimModel() { Id = x.Id, Name = x.Name, Value = x.Value, ÑomparisonMode = x.ÑomparisonMode, ConditionValueType = x.ConditionValueType, HardwareId = x.HardwareId, PortId = x.PortId }).ToArray(),
                ConditionsTypesEnum.Trigger => db.TrigersConditions.Where(x => x.OwnerId == condition_request.OwnerId).Select(x => new ConditionAnonimModel() { Id = x.Id, Name = x.Name, Value = x.Value, ÑomparisonMode = x.ÑomparisonMode, ConditionValueType = x.ConditionValueType, HardwareId = x.HardwareId, PortId = x.PortId }).ToArray(),
                _ => throw new NotImplementedException()
            };
        }
        return res_ac;
    }

    /// <summary>
    /// 
    /// </summary>
    [HttpDelete($"/{GlobalStatic.Routes.Conditions}/{{condition_id}}/{{condition_type}}")]
    public ConditionsAnonimResponseModel ConditionDelete(int condition_id, ConditionsTypesEnum condition_type)
    {
        ConditionsAnonimResponseModel res_ac = new();

        lock (ServerContext.DbLocker)
        {
            using ServerContext db = new();
            int owner_id;
            switch (condition_type)
            {
                case ConditionsTypesEnum.Command:
                    CommandConditionModelDB condition_db = db.ConditionsCommands.First(x => x.Id == condition_id);

                    owner_id = condition_db.OwnerId;
                    db.ConditionsCommands.RemoveRange(condition_db);
                    break;
                case ConditionsTypesEnum.Trigger:
                    TrigerConditionModelDB condition_dbt = db.TrigersConditions.First(x => x.Id == condition_id);

                    owner_id = condition_dbt.OwnerId;
                    db.TrigersConditions.Remove(condition_dbt);
                    break;
                default:
                    throw new NotImplementedException("{D7104662-A8D1-4A5B-B3B5-8433D05DA305}");
            }

            db.SaveChanges();

            res_ac.Conditions = condition_type switch
            {
                ConditionsTypesEnum.Command => db.ConditionsCommands.Where(x => x.OwnerId == owner_id).Select(x => new ConditionAnonimModel() { Id = x.Id, Name = x.Name, Value = x.Value, ÑomparisonMode = x.ÑomparisonMode, ConditionValueType = x.ConditionValueType, HardwareId = x.HardwareId, PortId = x.PortId }).ToArray(),
                ConditionsTypesEnum.Trigger => db.TrigersConditions.Where(x => x.OwnerId == owner_id).Select(x => new ConditionAnonimModel() { Id = x.Id, Name = x.Name, Value = x.Value, ÑomparisonMode = x.ÑomparisonMode, ConditionValueType = x.ConditionValueType, HardwareId = x.HardwareId, PortId = x.PortId }).ToArray(),
                _ => throw new NotImplementedException()
            };
        }

        return res_ac;
    }

}