////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using ab.context;
using Microsoft.EntityFrameworkCore;
using SharedLib;

namespace ServerLib;

/// <summary>
/// Пользователи
/// </summary>
public class ConditionsLocalService(IDbContextFactory<ServerContext> DbFactory) : IConditionsService
{
    /// <inheritdoc/>
    public async Task<ConditionsAnonimResponseModel> ConditionDelete(int condition_id, ConditionsTypesEnum condition_type, CancellationToken cancellation_token = default)
    {
        ConditionsAnonimResponseModel res_ac = new();
        using ServerContext db = await DbFactory.CreateDbContextAsync(cancellation_token);
        lock (ServerContext.DbLocker)
        {
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
                ConditionsTypesEnum.Command => db.ConditionsCommands.Where(x => x.OwnerId == owner_id).Select(x => new ConditionAnonimModel() { Id = x.Id, Name = x.Name, Value = x.Value, СomparisonMode = x.СomparisonMode, ConditionValueType = x.ConditionValueType, HardwareId = x.HardwareId, PortId = x.PortId }).ToArray(),
                ConditionsTypesEnum.Trigger => db.TrigersConditions.Where(x => x.OwnerId == owner_id).Select(x => new ConditionAnonimModel() { Id = x.Id, Name = x.Name, Value = x.Value, СomparisonMode = x.СomparisonMode, ConditionValueType = x.ConditionValueType, HardwareId = x.HardwareId, PortId = x.PortId }).ToArray(),
                _ => throw new NotImplementedException()
            };
        }

        return res_ac;
    }

    /// <inheritdoc/>
    public async Task<ConditionsAnonimResponseModel> ConditionsGetByOwner(int owner_id, ConditionsTypesEnum condition_type, CancellationToken cancellation_token = default)
    {
        ConditionsAnonimResponseModel res_ac = new();
        using ServerContext db = await DbFactory.CreateDbContextAsync(cancellation_token);
        lock (ServerContext.DbLocker)
        {
            res_ac.Conditions = condition_type switch
            {
                ConditionsTypesEnum.Command => db.ConditionsCommands.Where(x => x.OwnerId == owner_id).Select(x => new ConditionAnonimModel() { Id = x.Id, Name = x.Name, Value = x.Value, СomparisonMode = x.СomparisonMode, ConditionValueType = x.ConditionValueType, HardwareId = x.HardwareId, PortId = x.PortId }).ToArray(),
                ConditionsTypesEnum.Trigger => db.TrigersConditions.Where(x => x.OwnerId == owner_id).Select(x => new ConditionAnonimModel() { Id = x.Id, Name = x.Name, Value = x.Value, СomparisonMode = x.СomparisonMode, ConditionValueType = x.ConditionValueType, HardwareId = x.HardwareId, PortId = x.PortId }).ToArray(),
                _ => throw new NotImplementedException()
            };
        }
        return res_ac;
    }

    /// <inheritdoc/>
    public async Task<ConditionsAnonimResponseModel> ConditionUpdateOrCreate(ConditionUpdateModel condition_request, CancellationToken cancellation_token = default)
    {
        ConditionsAnonimResponseModel res_ac = new();
        if (condition_request is null)
        {
            res_ac.AddError("Ошибка выполнения запроса: {A6A02232-9F35-4BA3-AF50-D8767A5C2FC9}");
            return res_ac;
        }
        ResponseBaseModel check = condition_request.ValidateCondition;
        if (!check.IsSuccess)
            return (ConditionsAnonimResponseModel)check;

        if (string.IsNullOrWhiteSpace(condition_request.Name))
        {
            res_ac.AddWarning($"Для элемента{(condition_request.Id > 0 ? $" #{condition_request.Id}" : "")} не указано имя (");
        }

        if (condition_request.ConditionValueType == СomparisonsValuesTypesEnum.ValueAsDecimal && (condition_request.СomparisonMode == СomparisonsModesEnum.Equal || condition_request.СomparisonMode == СomparisonsModesEnum.NotEqual))
        {
            res_ac.AddInfo($"Для элемента{(condition_request.Id > 0 ? $" #{condition_request.Id}" : "")} установлено строгое числовое соответсвие. Попадание в такое условие маловероятно!");
        }
        using ServerContext db = await DbFactory.CreateDbContextAsync(cancellation_token);
        lock (ServerContext.DbLocker)
        {
            switch (condition_request.ConditionType)
            {
                case ConditionsTypesEnum.Command:
                    if (db.ConditionsCommands.Any(x => x.HardwareId == condition_request.HardwareId && x.PortId == condition_request.PortId && x.OwnerId == condition_request.OwnerId && x.Id != condition_request.Id))
                    {
                        res_ac.AddError($"Этот контроллер/порт уже используются");
                        return res_ac;
                    }
                    break;
                case ConditionsTypesEnum.Trigger:
                    if (db.TrigersConditions.Any(x => x.HardwareId == condition_request.HardwareId && x.PortId == condition_request.PortId && x.OwnerId == condition_request.OwnerId && x.Id != condition_request.Id))
                    {
                        res_ac.AddError($"Этот контроллер/порт уже используются");
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
                res_ac.AddSuccess($"Элемент #{condition_request.Id} обновлён");
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
                res_ac.AddSuccess($"Элемент создан");
            }
            db.SaveChanges();

            res_ac.Conditions = condition_request.ConditionType switch
            {
                ConditionsTypesEnum.Command => db.ConditionsCommands.Where(x => x.OwnerId == condition_request.OwnerId).Select(x => new ConditionAnonimModel() { Id = x.Id, Name = x.Name, Value = x.Value, СomparisonMode = x.СomparisonMode, ConditionValueType = x.ConditionValueType, HardwareId = x.HardwareId, PortId = x.PortId }).ToArray(),
                ConditionsTypesEnum.Trigger => db.TrigersConditions.Where(x => x.OwnerId == condition_request.OwnerId).Select(x => new ConditionAnonimModel() { Id = x.Id, Name = x.Name, Value = x.Value, СomparisonMode = x.СomparisonMode, ConditionValueType = x.ConditionValueType, HardwareId = x.HardwareId, PortId = x.PortId }).ToArray(),
                _ => throw new NotImplementedException()
            };
        }
        return res_ac;
    }
}