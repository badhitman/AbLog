////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using ab.context;
using Microsoft.EntityFrameworkCore;
using SharedLib;

namespace ServerLib;

/// <summary>
/// Конкуренции команд скриптов
/// </summary>
public class ContentionsLocalService(IDbContextFactory<ServerContext> DbFactory) : IContentionsService
{
    /// <inheritdoc/>
    public async Task<IdsResponseModel> ContentionSet(ContentionUpdateModel contention_json, CancellationToken cancellation_token = default)
    {
        IdsResponseModel res_ids = new();
        if (contention_json is null)
        {
            res_ids.AddError("Ошибка выполнения запроса: {B642799D-9335-4A62-855C-2949E5CA9B20}");
            return res_ids;
        }
        using ServerContext db = await DbFactory.CreateDbContextAsync(cancellation_token);
        lock (ServerContext.DbLocker)
        {
            ScriptModelDB? script_db = db.Scripts.Include(x => x.Commands).Include(x => x.Contentions).FirstOrDefault(x => x.Id == contention_json.ScriptMasterId);
            if (script_db?.Commands is null || script_db?.Contentions is null)
            {
                res_ids.AddError("Ошибка выполнения запроса: {95E6069A-6E9A-4966-84D4-EA03D87AA085}");
                return res_ids;
            }
            bool contain_slave = script_db?.Contentions?.Any(x => x.SlaveScriptId == contention_json.ScriptSlaveId) == true;
            if ((contention_json.IsSet && contain_slave) || (!contention_json.IsSet && !contain_slave))
            {
                res_ids.AddInfo("Уже установлено");
                res_ids.Ids = script_db!.Contentions.Select(x => x.Id).ToArray();
                return res_ids;
            }

            if (contention_json.IsSet)
            {
                db.Contentions.Add(new ContentionsModelDB() { MasterScriptId = contention_json.ScriptMasterId, SlaveScriptId = contention_json.ScriptSlaveId });
            }
            else
            {
                db.Remove(db.Contentions.First(x => x.MasterScriptId == contention_json.ScriptMasterId && x.SlaveScriptId == contention_json.ScriptSlaveId));
            }
            db.SaveChanges();
            script_db = db.Scripts.Include(x => x.Contentions).FirstOrDefault(x => x.Id == contention_json.ScriptMasterId);
            res_ids.Ids = script_db!.Contentions!.Select(x => x.SlaveScriptId).ToArray();
        }

        return res_ids;
    }
    /// <inheritdoc/>

    public async Task<IdsResponseModel> ContentionsGetByScript(int script_id, CancellationToken cancellation_token = default)
    {
        IdsResponseModel res_ids = new();
        using ServerContext db = await DbFactory.CreateDbContextAsync(cancellation_token);
        lock (ServerContext.DbLocker)
        {

            ScriptModelDB? script_db = db.Scripts.Include(x => x.Contentions).FirstOrDefault(x => x.Id == script_id);
            if (script_db?.Contentions is null)
            {
                res_ids.AddError("Ошибка выполнения запроса: {2738D41C-CE83-4394-8669-EDFB13C82852}");
                return res_ids;
            }
            res_ids.Ids = script_db.Contentions.Select(x => x.SlaveScriptId).ToArray();
        }

        return res_ids;
    }
}