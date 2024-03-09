////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using ab.context;
using Microsoft.EntityFrameworkCore;
using SharedLib;
using System.Diagnostics;

namespace ServerLib;

/// <summary>
/// 
/// </summary>
public class SystemCommandsLocalService(IDbContextFactory<ServerContext> DbFactory) : ISystemCommandsService
{
    /// <inheritdoc/>
    public Task<ResponseBaseModel> CommandDelete(int comm_id, CancellationToken cancellation_token = default)
    {
        ResponseBaseModel res = new();
        using ServerContext db = DbFactory.CreateDbContext();

        lock (ServerContext.DbLocker)
        {
            SystemCommandModelDB? com_db = db.SystemCommands.FirstOrDefault(x => x.Id == comm_id);
            if (com_db is null)
            {
                res.AddError("com_db is null is null. error {F6DF81C1-5361-4BE3-AC5F-76F4AC457EB0}");
                return Task.FromResult(res);
            }
            db.Remove(com_db);
            db.SaveChanges();
            res.AddSuccess("Команда удалена");
        }

        return Task.FromResult(res);
    }

    /// <inheritdoc/>
    public Task<SystemCommandsResponseModel> CommandsGetAll(CancellationToken cancellation_token = default)
    {
        SystemCommandsResponseModel res = new();
        using ServerContext db = DbFactory.CreateDbContext();

        lock (ServerContext.DbLocker)
        {
            res.SystemCommands = [.. db.SystemCommands];
        }

        return Task.FromResult(res);
    }

    /// <inheritdoc/>
    public Task<ResponseBaseModel> CommandUpdateOrCreate(SystemCommandModelDB comm, CancellationToken cancellation_token = default)
    {
        comm.Name = comm.Name.Trim();
        ResponseBaseModel res = new();
        if (string.IsNullOrWhiteSpace(comm.FileName) || string.IsNullOrWhiteSpace(comm.Name))
        {
            res.AddError("Поля 'Name' и 'FileName' обязательны для заполнения. error {24FCBE74-127A-473E-9577-1EA58C9CA7E8}");
            return Task.FromResult(res);
        }

        using ServerContext db = DbFactory.CreateDbContext();

        lock (ServerContext.DbLocker)
        {
            SystemCommandModelDB? control_com = db.SystemCommands
                .Where(x => x.Id != comm.Id)
                        .FirstOrDefault(x => x.FileName == comm.FileName && x.Arguments == comm.Arguments);

            if (control_com is not null)
            {
                res.AddError($"Такая команда уже существует: #{control_com.Id}");
                return Task.FromResult(res);
            }

            control_com = db.SystemCommands
                .Where(x => x.Id != comm.Id)
                        .FirstOrDefault(x => x.Name == comm.Name);

            if (control_com is not null)
            {
                res.AddError($"Такое имя команды уже существует: #{control_com.Id}");
                return Task.FromResult(res);
            }

            SystemCommandModelDB? com_db = db.SystemCommands.FirstOrDefault(x => x.Id == comm.Id);
            if (com_db is null)
            {
                if (comm.Id > 0)
                {
                    res.AddError("com_db is null is null. error {D278D14F-192A-4AC3-A29C-0F12A1E4560B}");
                    return Task.FromResult(res);
                }
                else
                {
                    db.Add(comm);
                }
            }
            else
            {
                com_db.IsDisabled = comm.IsDisabled;
                com_db.Name = comm.Name;
                com_db.FileName = comm.FileName;
                com_db.Arguments = comm.Arguments;
                db.Update(com_db);
            }
            db.SaveChanges();
            res.AddSuccess("Команда обновлена/сохранена");
        }

        return Task.FromResult(res);
    }

    /// <inheritdoc/>
    public Task<ResponseBaseModel> CommandRun(int comm_id, CancellationToken cancellation_token = default)
    {
        ResponseBaseModel res = new();
        using ServerContext db = DbFactory.CreateDbContext();

        lock (ServerContext.DbLocker)
        {
            SystemCommandModelDB? com_db = db.SystemCommands.FirstOrDefault(x => x.Id == comm_id);
            if (com_db is null)
            {
                res.AddError("com_db is null is null. error {CD013C5C-6930-41D0-A8E8-2D3E2FF2FD08}");
                return Task.FromResult(res);
            }

            Process? p = Process.Start(new ProcessStartInfo() { FileName = com_db.FileName, Arguments = com_db.Arguments ?? "" });

            if (p is null)
            {
                res.AddError("Process is null. error {F9D53DCB-BE75-4D66-8A85-99F0EA6908C1}");

                return Task.FromResult(res);
            }

            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = Path.GetTempFileName();
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            res.AddSuccess($"Команда выполнена:\n{output}");
        }

        return Task.FromResult(res);
    }
}