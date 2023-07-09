////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Mvc;
using ab.context;
using SharedLib;

namespace ABLog;

/// <summary>
/// 
/// </summary>
[ApiController]
[Route("/api/[controller]")]
public class TriggersController : ControllerBase
{
    private readonly ILogger<TriggersController> _logger;

    /// <summary>
    /// 
    /// </summary>
    public TriggersController(ILogger<TriggersController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// 
    /// </summary>
    [HttpGet($"{GlobalStatic.Routes.LIST}")]
    public TriggersResponseModel TriggersGetAll()
    {
        TriggersResponseModel res_trigs = new();
        lock (ServerContext.DbLocker)
        {
            using ServerContext db = new();
            res_trigs.Triggers = db.Trigers.ToArray();
        }

        return res_trigs;
    }

    /// <summary>
    /// 
    /// </summary>
    [HttpPost($"{GlobalStatic.Routes.UPDATE}")]
    public TriggersResponseModel TriggerUpdateOrCreate(TrigerModelDB trigger_json)
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

        lock (ServerContext.DbLocker)
        {
            using ServerContext db = new();
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
                    res_trigs.Triggers = db.Trigers.ToArray();
                    return res_trigs;
                }

                if (db.Trigers.Any(x => x.Name.ToLower() == trigger_json.Name.ToLower() && x.Id != trigger_db.Id))
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

            res_trigs.Triggers = db.Trigers.ToArray();
        }

        return res_trigs;
    }

    /// <summary>
    /// 
    /// </summary>
    [HttpDelete("{trigger_id}")]
    public TriggersResponseModel TriggerDelete(int trigger_id)
    {
        TriggersResponseModel res_trigs = new();

        lock (ServerContext.DbLocker)
        {
            using ServerContext db = new();

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
}