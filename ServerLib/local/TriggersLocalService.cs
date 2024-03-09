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
public class TriggersLocalService(IDbContextFactory<ServerContext> DbFactory) : ITriggersService
{
    /// <inheritdoc/>
    public async Task<TriggersResponseModel> TriggerDelete(int trigger_id, CancellationToken cancellation_token = default)
    {
        TriggersResponseModel res_trigs = new();

        using ServerContext db = await DbFactory.CreateDbContextAsync(cancellation_token);
        lock (ServerContext.DbLocker)
        {

            TrigerModelDB? trigger_db = db.Trigers.FirstOrDefault(x => x.Id == trigger_id);

            if (trigger_db is null)
            {
                res_trigs.AddError("Ошибка выполнения запроса: {FA018824-5F11-423B-A0DE-7F1E561464A0}");
                return res_trigs;
            }
            db.Remove(trigger_db);
            db.SaveChanges();
            res_trigs.Triggers = db.Trigers.ToArray();
        }

        return res_trigs;
    }

    /// <inheritdoc/>
    public async Task<TriggersResponseModel> TriggersGetAll(CancellationToken cancellation_token = default)
    {
        TriggersResponseModel res_trigs = new();
        using ServerContext db = await DbFactory.CreateDbContextAsync(cancellation_token);
        lock (ServerContext.DbLocker)
        {
            res_trigs.Triggers = [.. db.Trigers];
        }

        return res_trigs;
    }

    /// <inheritdoc/>
    public async Task<TriggersResponseModel> TriggerUpdateOrCreate(TrigerModelDB trigger_json, CancellationToken cancellation_token = default)
    {
        TriggersResponseModel res_trigs = new();

        if (trigger_json is null)
        {
            res_trigs.AddError("Ошибка выполнения запроса: {1A6B76A2-70C5-4F65-BB92-97508AA67395}");
            return res_trigs;
        }

        if (trigger_json.ScriptId < 1)
        {
            res_trigs.AddError("Укажите скрипт для автоматического запуска");
            return res_trigs;
        }

        using ServerContext db = await DbFactory.CreateDbContextAsync(cancellation_token);
        lock (ServerContext.DbLocker)
        {
            if (trigger_json.Id > 0)
            {
                TrigerModelDB? trigger_db = null;

                trigger_db = db.Trigers.FirstOrDefault(x => x.Id == trigger_json.Id);
                if (trigger_db is null)
                {
                    res_trigs.AddError("Ошибка выполнения запроса: {A7D8E5AF-2046-422E-ABB9-2E1C9288EFE2}");
                    return res_trigs;
                }
                if (trigger_db.Name == trigger_json.Name && trigger_db.Description == trigger_json.Description && trigger_db.IsDisable == trigger_json.IsDisable && trigger_db.ScriptId == trigger_json.ScriptId)
                {
                    res_trigs.AddInfo("Сохранение не требуется");
                    res_trigs.Triggers = [.. db.Trigers];
                    return res_trigs;
                }

                if (db.Trigers.Any(x => x.Name.Equals(trigger_json.Name, StringComparison.CurrentCultureIgnoreCase) && x.Id != trigger_db.Id))
                {
                    res_trigs.AddError("Имя тригера не уникально. Задайте другое имя.");
                    return res_trigs;
                }

                trigger_db.Name = trigger_json.Name;
                trigger_db.Description = trigger_json.Description;
                trigger_db.IsDisable = trigger_json.IsDisable;
                trigger_db.ScriptId = trigger_json.ScriptId;

                db.Update(trigger_db);
                db.SaveChanges();
                res_trigs.AddSuccess($"Тригер #{trigger_db.Id} обновлён");
            }
            else
            {
                trigger_json.Id = 0;
                db.Add(trigger_json);
                db.SaveChanges();
                res_trigs.AddSuccess("Тригер создан");
            }

            res_trigs.Triggers = [.. db.Trigers];
        }

        return res_trigs;
    }
}