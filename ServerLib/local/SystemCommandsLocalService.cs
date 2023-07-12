////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using System.Diagnostics;
using ab.context;
using SharedLib;

namespace ServerLib;

/// <summary>
/// 
/// </summary>
public class SystemCommandsLocalService : ISystemCommandsService
{
    /// <inheritdoc/>
    public Task<ResponseBaseModel> CommandDelete(int comm_id, CancellationToken cancellation_token = default)
    {
        ResponseBaseModel res = new();
        using ServerContext _context = new();

        lock (ServerContext.DbLocker)
        {
            SystemCommandModelDB? com_db = _context.SystemCommands.FirstOrDefault(x => x.Id == comm_id);
            if (com_db is null)
            {
                res.AddError("com_db is null is null. error {F6DF81C1-5361-4BE3-AC5F-76F4AC457EB0}");
                return Task.FromResult(res);
            }
            _context.Remove(com_db);
            _context.SaveChanges();
            res.AddSuccess("Команда удалена");
        }

        return Task.FromResult(res);
    }

    /// <inheritdoc/>
    public Task<SystemCommandsResponseModel> CommandsGetAll(CancellationToken cancellation_token = default)
    {
        SystemCommandsResponseModel res = new();
        using ServerContext _context = new();

        lock (ServerContext.DbLocker)
        {
            res.SystemCommands = _context.SystemCommands.ToArray();
        }

        return Task.FromResult(res);
    }

    /// <inheritdoc/>
    public Task<ResponseBaseModel> CommandUpdateOrCreate(SystemCommandModelDB comm, CancellationToken cancellation_token = default)
    {
        ResponseBaseModel res = new();
        if (!comm.IsCorrect)
        {
            res.AddError("Обязательные поля не заполнены: 'Name' и 'FileName'. error {24FCBE74-127A-473E-9577-1EA58C9CA7E8}");
            return Task.FromResult(res);
        }

        using ServerContext _context = new();

        lock (ServerContext.DbLocker)
        {
            SystemCommandModelDB? com_db = _context.SystemCommands.FirstOrDefault(x => x.Id == comm.Id);
            if (com_db is null)
            {
                if (comm.Id > 0)
                {
                    res.AddError("com_db is null is null. error {D278D14F-192A-4AC3-A29C-0F12A1E4560B}");
                    return Task.FromResult(res);
                }
                else
                {
                    _context.Add(comm);
                }
            }
            else
                _context.Update(comm);

            _context.SaveChanges();
            res.AddSuccess("Команда обновлена/сохранена");
        }

        return Task.FromResult(res);
    }

    /// <inheritdoc/>
    public Task<ResponseBaseModel> CommandRun(int comm_id, CancellationToken cancellation_token = default)
    {
        ResponseBaseModel res = new();
        using ServerContext _context = new();

        lock (ServerContext.DbLocker)
        {
            SystemCommandModelDB? com_db = _context.SystemCommands.FirstOrDefault(x => x.Id == comm_id);
            if (com_db is null)
            {
                res.AddError("com_db is null is null. error {CD013C5C-6930-41D0-A8E8-2D3E2FF2FD08}");
                return Task.FromResult(res);
            }

            Process? proc = Process.Start(new ProcessStartInfo() { FileName = com_db.FileName, Arguments = com_db.Arguments ?? "" });
            res.AddSuccess("Команда выполнена");
        }

        return Task.FromResult(res);
    }
}