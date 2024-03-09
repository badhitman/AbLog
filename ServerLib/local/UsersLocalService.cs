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
public class UsersLocalService(IDbContextFactory<ServerContext> DbFactory) : IUsersService
{
    /// <inheritdoc/>
    public Task<UserResponseModel> GetUser(long telegram_id, CancellationToken cancellation_token = default)
    {
        using ServerContext db = DbFactory.CreateDbContext();
        UserResponseModel res = new();
        lock (ServerContext.DbLocker)
        {
            res.User = db.Users.FirstOrDefault(x => x.TelegramId == telegram_id);
        }
        if (res.User is null)
            res.AddError("User is null. error {E2204B76-5A95-4A49-B7CF-8B5216238FFE}");

        return Task.FromResult(res);
    }

    /// <inheritdoc/>
    public Task<UsersPaginationResponseModel> UsersGetList(UserListGetModel req, CancellationToken cancellation_token = default)
    {
        if (req.PageSize < 5)
            req.PageSize = 5;
        if (req.PageSize > 100)
            req.PageSize = 100;
        if (req.PageNum < 0)
            req.PageNum = 0;

        using ServerContext db = DbFactory.CreateDbContext();
        IQueryable<UserModelDB> query = db.Users.AsQueryable();

        if (!req.IncludeDisabledUsers)
            query = query.Where(x => !x.IsDisabled);

        UsersPaginationResponseModel res = new()
        {
            TotalRowsCount = query.Count(),
            PageSize = req.PageSize,
            PageNum = req.PageNum
        };
        int pages_all_count = (int)Math.Ceiling((double)res.TotalRowsCount / (double)req.PageSize);

        res.Users = [.. query
            .Skip((req.PageNum) * req.PageSize)
            .Take(req.PageSize)];

        res.AddInfo($"Пользователи: {res.Users.Count()} из {res.TotalRowsCount}");

        return Task.FromResult(res);
    }

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdateUser(UpdateUserModel req, CancellationToken cancellation_token = default)
    {
        ResponseBaseModel res = new();
        using ServerContext db = DbFactory.CreateDbContext();
        UserModelDB? user_db;
        lock (ServerContext.DbLocker)
        {
            user_db = db.Users.FirstOrDefault(x => x.TelegramId == req.TelegramId);
            if (user_db is null)
            {
                res.AddError("user_db is null. error {D0694672-D5EE-43F4-A158-CCC7C2748729}");
                return Task.FromResult(res);
            }

            if (user_db.Email == req.Email && user_db.AllowChangeMqttConfig == req.AllowChangeMqttConfig && user_db.AllowSystemCommands == req.AllowSystemCommands && user_db.IsDisabled == req.IsDisabled && user_db.AlarmSubscriber == req.AllowAlerts && user_db.CommandsAllowed == req.AllowHardwareControl)
            {
                res.AddInfo("Изменений нет");
                return Task.FromResult(res);
            }
            user_db.AllowChangeMqttConfig = req.AllowChangeMqttConfig;
            user_db.AllowSystemCommands = req.AllowSystemCommands;
            user_db.IsDisabled = req.IsDisabled;
            user_db.Email = req.Email;
            user_db.AlarmSubscriber = req.AllowAlerts;
            user_db.CommandsAllowed = req.AllowHardwareControl;
            db.Update(user_db);
            db.SaveChanges();
            res.AddSuccess("Изменения сохранены в БД");
        }

        return Task.FromResult(res);
    }
}