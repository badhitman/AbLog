////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ab.context;
using SharedLib;

namespace ABLog;

/// <summary>
/// 
/// </summary>
[ApiController]
[Route("/api/[controller]")]
public class CommandsController : ControllerBase
{
    private readonly ILogger<CommandsController> _logger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    public CommandsController(ILogger<CommandsController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// 
    /// </summary>
    [HttpGet($"{GlobalStatic.Routes.BY_OWNER}/{{script_id}}")]
    public EntriesSortingResponseModel GetCommandsEntriesByScript(int script_id)
    {
        EntriesSortingResponseModel res_oe = new();
        lock (ServerContext.DbLocker)
        {
            using ServerContext db = new();
            ScriptModelDB? db_script = db.Scripts.Include(x => x.Commands).FirstOrDefault(x => x.Id == script_id);
            if (db_script?.Commands is null)
            {
                res_oe.AddError("Ошибка выполнения запроса: {7E6A8D50-3C9C-4BC0-A790-8001A9D3F7B4}");
                return res_oe;
            }
            res_oe.Entries = db_script.Commands.OrderBy(x => x.Sorting).Select(x => new EntrySortingModel() { Id = x.Id, Name = x.Name, Sorting = x.Sorting }).ToArray();
        }
        return res_oe;
    }

    /// <summary>
    /// 
    /// </summary>
    [HttpGet("{command_id}")]
    public CommandResponseModel CommandGet(int command_id)
    {
        CommandResponseModel res_c = new();
        lock (ServerContext.DbLocker)
        {
            using ServerContext db = new();
            res_c.Command = db.Commands.Include(x => x.Conditions).FirstOrDefault(x => x.Id == command_id);
            if (res_c.Command is null)
                res_c.AddError("Ошибка выполнения запроса: {29F5A45E-B0C0-4DDD-B135-39E5BC735028}");
        }
        return res_c;
    }

    /// <summary>
    /// 
    /// </summary>
    [HttpPost($"{GlobalStatic.Routes.UPDATE}")]
    public ResponseBaseModel CommandUpdateOrCreate(CommandModelDB command_json)
    {
        ResponseBaseModel res_b = new();
        if (command_json is null)
        {
            res_b.AddError("Ошибка выполнения запроса: {DF0776E0-FD60-4BF6-88CA-18497927FD03}");
            return res_b;
        }

        command_json.ExecutionParametr = command_json.ExecutionParametr?.Trim();
        switch (command_json.CommandType)
        {
            case TypesCommandsEnum.Controller:
                if (string.IsNullOrWhiteSpace(command_json.ExecutionParametr))
                    res_b.AddError("Не указана команда контроллера");
                if (command_json.Execution < 1)
                    res_b.AddError("Не указан контроллер");
                if (!res_b.IsSuccess)
                    return res_b;

                lock (ServerContext.DbLocker)
                {
                    using ServerContext db = new();
                    HardwareModelDB? hw_db = db.Hardwares.FirstOrDefault(x => x.Id == command_json.Execution);
                    if (hw_db is null)
                    {
                        res_b.AddError($"Не найден контроллер {command_json.Execution}");
                        return res_b;
                    }
                }
                break;
            case TypesCommandsEnum.Port:
                if (command_json.Execution < 1)
                    res_b.AddError("Не указан идентификатор порта");

                lock (ServerContext.DbLocker)
                {
                    using ServerContext db = new();
                    PortModelDB? port_hw = db.Ports.FirstOrDefault(x => x.Id == command_json.Execution);
                    if (port_hw is null)
                    {
                        res_b.AddError($"Не найден порт {command_json.Execution}");
                        return res_b;
                    }
                }
                break;
            case TypesCommandsEnum.Exit:
                lock (ServerContext.DbLocker)
                {


                }
                break;
        }

        if (!res_b.IsSuccess)
        {
            return res_b;
        }

        lock (ServerContext.DbLocker)
        {
            using ServerContext db = new();
            if (command_json.Id > 0)
            {
                CommandModelDB? com_db = null;

                com_db = db.Commands.FirstOrDefault(x => x.Id == command_json.Id);
                if (com_db is null)
                {
                    res_b.AddError("Ошибка выполнения запроса: {68B5B0D5-CA5C-4C2B-AEB8-645CB6593D7B}");
                    return res_b;
                }

                if (db.Commands.Any(x => x.ScriptId == command_json.ScriptId && x.Name.ToLower() == command_json.Name.ToLower() && x.Id != com_db.Id))
                {
                    res_b.AddError("Имя скрипта не уникально. Задайте другое имя.");
                    return res_b;
                }

                com_db.Name = command_json.Name;
                com_db.CommandType = command_json.CommandType;
                com_db.Conditions = command_json.Conditions;
                com_db.Execution = command_json.Execution;
                com_db.ExecutionParametr = command_json.ExecutionParametr;
                com_db.Hidden = command_json.Hidden;
                com_db.PauseSecondsBeforeExecution = command_json.PauseSecondsBeforeExecution;
                com_db.ScriptId = command_json.ScriptId;
                com_db.Sorting = command_json.Sorting;

                db.Update(com_db);
                db.SaveChanges();
            }
            else
            {
                CommandModelDB com_new = new()
                {
                    Name = command_json.Name,
                    CommandType = command_json.CommandType,
                    Conditions = command_json.Conditions,
                    Execution = command_json.Execution,
                    ExecutionParametr = command_json.ExecutionParametr,
                    Hidden = command_json.Hidden,
                    PauseSecondsBeforeExecution = command_json.PauseSecondsBeforeExecution,
                    ScriptId = command_json.ScriptId,
                    Sorting = command_json.Sorting
                };
                db.Add(com_new);
                db.SaveChanges();
            }
        }

        return res_b;
    }

    /// <summary>
    /// 
    /// </summary>
    [HttpPut($"{GlobalStatic.Routes.SORTING}")]
    public EntriesSortingResponseModel CommandSortingSet(IdsPairModel req)
    {
        EntriesSortingResponseModel res_oe = new();
        if (req is null)
        {
            res_oe.AddError("Ошибка выполнения запроса: {3CA6C382-45D7-4B4A-BD50-CDD1D630CE16}");
            return res_oe;
        }

        if (req.Id1 < 1 || req.Id2 < 1)
        {
            res_oe.AddError("Ошибка выполнения запроса: {A6E3F92B-7AB7-4257-9F2A-47A1ECE67598}");
            return res_oe;
        }

        lock (ServerContext.DbLocker)
        {
            using ServerContext db = new();
            var c1 = db.Commands.FirstOrDefault(x => x.Id == req.Id1);
            var c2 = db.Commands.FirstOrDefault(x => x.Id == req.Id2);

            if (c1 is null || c2 is null || c1.ScriptId != c2.ScriptId)
            {
                res_oe.AddError("Ошибка выполнения запроса: {6E07CBF8-A54D-4FF5-A5A9-F60D2FBA327B}");
                return res_oe;
            }
            double s = c1.Sorting;
            c1.Sorting = c2.Sorting;
            c2.Sorting = s;

            db.UpdateRange(c1, c2);
            db.SaveChanges();

            res_oe.Entries = db.Commands
                .Where(x => x.ScriptId == c1.ScriptId)
                .OrderBy(x => x.Sorting)
                .Select(x => new EntrySortingModel() { Id = x.Id, Name = x.Name, Sorting = x.Sorting })
                .ToArray();
        }

        return res_oe;
    }

    /// <summary>
    /// 
    /// </summary>
    [HttpDelete("{id_command}")]
    public ResponseBaseModel CommandDelete(int id_command)
    {
        ResponseBaseModel res_b = new();

        lock (ServerContext.DbLocker)
        {
            using ServerContext db = new();
            CommandModelDB? command_db = db.Commands.FirstOrDefault(x => x.Id == id_command);
            if (command_db is null)
            {
                res_b.AddError("Ошибка выполнения запроса: {9D2B233F-AB66-4F76-9C28-871C3EE46933}");
                return res_b;
            }

            db.Remove(command_db);
            db.SaveChanges();
            res_b.AddSuccess($"Команда #{id_command} удалена");
        }

        return res_b;
    }
}