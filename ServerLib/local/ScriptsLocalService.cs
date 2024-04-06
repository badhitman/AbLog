////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using ab.context;
using Microsoft.EntityFrameworkCore;
using SharedLib;

namespace ServerLib;

/// <summary>
/// Скрипты
/// </summary>
public class ScriptsLocalService(IDbContextFactory<ServerContext> DbFactory) : IScriptsService
{
    /// <inheritdoc/>
    public async Task<ScriptsResponseModel> ScriptsGetAll(CancellationToken cancellation_token = default)
    {
        ScriptsResponseModel res_s = new();
        using ServerContext db = await DbFactory.CreateDbContextAsync(cancellation_token);
        lock (ServerContext.DbLocker)
        {
            res_s.Scripts = [.. db.Scripts.Include(x => x.Commands)];
        }
        return res_s;
    }

    /// <inheritdoc/>
    public async Task<ScriptsResponseModel> ScriptDelete(int script_id, CancellationToken cancellation_token = default)
    {
        ScriptsResponseModel res_s = new();
        using ServerContext db = await DbFactory.CreateDbContextAsync(cancellation_token);
        lock (ServerContext.DbLocker)
        {
            ScriptModelDB? script_db = db.Scripts.FirstOrDefault(x => x.Id == script_id);

            if (script_db is null)
            {
                res_s.AddError("Ошибка выполнения запроса: {8A06BD8B-22C6-4811-8CBF-F44A588D6CF8}");
                return res_s;
            }
            db.Remove(script_db);
            db.SaveChanges();
            res_s.Scripts = db.Scripts.Include(x => x.Commands).ToList();
        }

        return res_s;
    }

    /// <inheritdoc/>
    public async Task<ScriptsResponseModel> ScriptUpdateOrCreate(EntryDescriptionModel script, CancellationToken cancellation_token = default)
    {
        ScriptsResponseModel res_s = new();
        using ServerContext db = await DbFactory.CreateDbContextAsync(cancellation_token);
        lock (ServerContext.DbLocker)
        {
            if (script.Id > 0)
            {
                ScriptModelDB? script_db = db.Scripts.FirstOrDefault(x => x.Id == script.Id);

                if (script_db is null)
                {
                    res_s.AddError("Ошибка выполнения запроса: {578EF286-706B-43F9-8058-A3BF47618B83}");
                    return res_s;
                }
                if (script_db.Name == script.Name && script_db.Description == script.Description)
                {
                    res_s.AddInfo("Сохранение не требуется");
                    res_s.Scripts = db.Scripts.Include(x => x.Commands).ToList();
                    return res_s;
                }

                if (db.Scripts.Any(x => x.Name.ToLower() == script.Name.ToLower() && x.Id != script_db.Id))
                {
                    res_s.AddError("Имя скрипта не уникально. Задайте другое имя.");
                    return res_s;
                }

                script_db.Name = script.Name;
                script_db.Description = script.Description;

                db.Update(script_db);
                db.SaveChanges();
            }
            else
            {
                ScriptModelDB script_new = new()
                {
                    Name = script.Name,
                    Description = script.Description
                };

                db.Add(script_new);
                db.SaveChanges();
            }

            res_s.Scripts = db.Scripts.Include(x => x.Commands).ToList();
        }

        return res_s;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ScriptEnableSet(ObjectStateModel req, CancellationToken cancellation_token = default)
    {
        ResponseBaseModel res = new();
        using ServerContext db = await DbFactory.CreateDbContextAsync(cancellation_token);
        lock (ServerContext.DbLocker)
        {
            ScriptModelDB? script_db = db.Scripts.FirstOrDefault(x => x.Id == req.Id);

            if (script_db is null)
            {
                res.AddError("Ошибка выполнения запроса: {8A06BD8B-22C6-4811-8CBF-F44A588D6CF8}");
                return res;
            }

            if (script_db.IsEnabled == req.State)
            {
                res.AddInfo("Обновления объекта не требуется");
                return res;
            }
            script_db.IsEnabled = req.State;

            db.Update(script_db);
            db.SaveChanges();
        }

        return res;
    }
}