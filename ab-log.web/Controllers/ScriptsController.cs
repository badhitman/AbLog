////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using ab.context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedLib;

namespace ABLogWeb;

/// <summary>
/// 
/// </summary>
[ApiController]
[Route("/api/[controller]")]
public class ScriptsController : ControllerBase
{
    private readonly ILogger<ScriptsController> _logger;

    /// <summary>
    /// 
    /// </summary>
    public ScriptsController(ILogger<ScriptsController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// 
    /// </summary>
    [HttpGet($"{GlobalStatic.Routes.LIST}")]
    public ScriptsResponseModel ScriptsGetAll()
    {
        ScriptsResponseModel res_s = new();
        lock (ServerContext.DbLocker)
        {
            using ServerContext db = new();
            res_s.Scripts = db.Scripts.Include(x => x.Commands).ToList();
        }
        return res_s;
    }

    /// <summary>
    /// 
    /// </summary>
    [HttpDelete("{script_id}")]
    public ScriptsResponseModel ScriptDelete(int script_id)
    {

        ScriptsResponseModel res_s = new();
        lock (ServerContext.DbLocker)
        {
            using ServerContext db = new();

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

    /// <summary>
    /// 
    /// </summary>
    [HttpPost($"{GlobalStatic.Routes.UPDATE}")]
    public ScriptsResponseModel ScriptUpdateOrCreate(EntryDescriptionModel script)
    {
        ScriptsResponseModel res_s = new();

        lock (ServerContext.DbLocker)
        {
            using ServerContext db = new();
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

    /// <summary>
    /// 
    /// </summary>
    [HttpPut($"{GlobalStatic.Routes.ENABLE}/{{script_id}}")]
    public ResponseBaseModel ScriptEnableSet([FromRoute] int script_id, bool is_enable)
    {
        ResponseBaseModel res = new();

        lock (ServerContext.DbLocker)
        {
            using ServerContext db = new();

            ScriptModelDB? script_db = db.Scripts.FirstOrDefault(x => x.Id == script_id);

            if (script_db is null)
            {
                res.AddError("Ошибка выполнения запроса: {8A06BD8B-22C6-4811-8CBF-F44A588D6CF8}");
                return res;
            }

            if (script_db.IsEnabled == is_enable)
            {
                res.AddInfo("Обновления объекта не требуется");
                return res;
            }
            script_db.IsEnabled = is_enable;

            db.Update(script_db);
            db.SaveChanges();
        }

        return res;
    }
}