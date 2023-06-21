using Microsoft.EntityFrameworkCore;
using SharedLib.IServices;
using ab.context;
using SharedLib;
using System.Net;
using Org.BouncyCastle.Ocsp;

namespace ServerLib
{
    /// <summary>
    /// Устройства
    /// </summary>
    public class HardwaresLocalService : IHardwaresService
    {
        /// <inheritdoc/>
        public async Task<HttpResponseModel> GetHardwareHtmlPage(HardvareGetRequestModel req)
        {
            HardwareModelDB? db_hw;
            lock (ServerContext.DbLocker)
            {
                using ServerContext db = new();
                db_hw = db.Hardwares.FirstOrDefault(x => x.Id == req.HardwareId);
                if (db_hw is null)
                    return (HttpResponseModel)ResponseBaseModel.CreateError("db.Hardwares.FirstOrDefault(x => x.Id == hardware_id) IS NULL: {98F67202-25A2-4D64-918B-72E651D63D15}");
            }

            if (string.IsNullOrWhiteSpace(db_hw.Address) || string.IsNullOrWhiteSpace(db_hw.Password))
                return (HttpResponseModel)ResponseBaseModel.CreateError("string.IsNullOrWhiteSpace(db_hw.Address) || string.IsNullOrWhiteSpace(db_hw.Password): {40003ADA-6DB8-48DD-88F8-EC1E05EF2CB1}");

            if (!db_hw.Address.EndsWith("/"))
                db_hw.Address += "/";

            if (!db_hw.Address.StartsWith("http"))
                db_hw.Address = $"http://{db_hw.Address}";

            if (req.Path?.StartsWith("/") == true)
                req.Path = req.Path[1..];

            try
            {
                using HttpClient client = new();
                client.BaseAddress = new Uri(db_hw.Address);
                string uri_path = string.IsNullOrWhiteSpace(req.Path)
                    ? db_hw.Password
                    : req.Path;

                if (!uri_path.StartsWith(db_hw.Password))
                    uri_path = $"{db_hw.Password}/{uri_path}";
                CancellationToken ct = new CancellationTokenSource(2000).Token;
                HttpResponseMessage response = await client.GetAsync(uri_path, ct);
                return new()
                {
                    StatusCode = response.StatusCode,
                    TextPayload = await response.Content.ReadAsStringAsync()
                };
            }
            catch (TaskCanceledException tcex)
            {
                return new()
                {
                    StatusCode = HttpStatusCode.RequestTimeout,
                    Messages = new List<ResultMessage>()
                    {
                        new ResultMessage() { TypeMessage = ResultTypeEnum.Warning, Text = $"Контроллер [{db_hw.Address}] недоступен" },
                        new ResultMessage() { TypeMessage = ResultTypeEnum.Error, Text = tcex.Message }
                    }
                };
            }
            catch (Exception ex)
            {
                return new()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Messages = new List<ResultMessage>() { new ResultMessage() { TypeMessage = ResultTypeEnum.Error, Text = ex.Message } }
                };
            }
        }

        /// <inheritdoc/>
        public Task<HardwareResponseModel> HardwareGet(int hardware_id)
        {
            HardwareResponseModel res_hw = new();

            lock (ServerContext.DbLocker)
            {
                using ServerContext db = new();
                HardwareModelDB? db_hw = db.Hardwares.FirstOrDefault(x => x.Id == hardware_id);
                if (db_hw is null)
                {
                    res_hw.AddError("Ошибка выполнения запроса: {771C7F32-36A7-4EC8-9F6C-7ED48D6FA99B}");
                    return Task.FromResult(res_hw);
                }
                res_hw.Hardware = new(db_hw);
            }
            return Task.FromResult(res_hw);
        }

        /// <inheritdoc/>
        public Task<PortHardwareResponseModel> HardwarePortGet(int port_id)
        {
            PortHardwareResponseModel res_port = new();
            lock (ServerContext.DbLocker)
            {
                using ServerContext db = new();
                PortModelDB? db_port = db.Ports.Include(x => x.Hardware).FirstOrDefault(x => x.Id == port_id);
                if (db_port is null)
                {
                    res_port.AddError("Ошибка выполнения запроса: {5AD81739-6A9B-4753-A47C-C278CD64705B}");
                    return Task.FromResult(res_port);
                }
                res_port.Port = new PortHardwareModel(db_port);
            }

            return Task.FromResult(res_port);
        }

        /// <inheritdoc/>
        public Task<HardwaresResponseModel> HardwaresGetAll()
        {
            HardwaresResponseModel res_hws = new();
            lock (ServerContext.DbLocker)
            {
                using ServerContext db = new();
                res_hws.Hardwares = db.Hardwares
                    .Include(x => x.Ports)
                    .ToArray()
                    .Select(x => new HardwareModel(x))
                    .ToArray();
            }
            return Task.FromResult(res_hws);
        }

        /// <inheritdoc/>
        public Task<EntriesResponseModel> HardwaresGetAllAsEntries()
        {
            EntriesResponseModel res_hws = new();
            lock (ServerContext.DbLocker)
            {
                using ServerContext db = new();
                res_hws.Entries = db.Hardwares.Select(x => new EntryModel() { Id = x.Id, Name = x.Name }).ToArray();
            }
            return Task.FromResult(res_hws);
        }

        /// <inheritdoc/>
        public Task<EntriesNestedResponseModel> HardwaresGetTreeNestedEntries()
        {
            EntriesNestedResponseModel res_tree_hw = new();
            lock (ServerContext.DbLocker)
            {
                using ServerContext db = new();

                res_tree_hw.Entries = db.Ports.Include(x => x.Hardware)
                    .Select(x => new { port_id = x.Id, port_num = x.PortNum, hw_id = x.HardwareId, hw_name = x.Hardware!.Name, hw_address = x.Hardware.Address })
                    .AsEnumerable()
                    .GroupBy(x => x.hw_id)
                    .Select(x =>
                    {
                        var curr_hw = x.First();

                        return new EntryNestedModel()
                        {
                            Id = curr_hw.hw_id,
                            Name = $"{curr_hw.hw_name}/{curr_hw.hw_address}",
                            Childs = x.Select(y => new EntryModel() { Id = y.port_id, Name = $"{y.port_id}#{y.port_num}" }).ToArray()
                        };
                    })
                    .ToArray();
            }
            return Task.FromResult(res_tree_hw);
        }

        /// <inheritdoc/>
        public Task<EntriyResponseModel> CheckPortHardware(PortHardwareCheckRequestModel req)
        {
            EntriyResponseModel res = new();
            if (req.HardwareId <= 0 || req.PortNum < 0)
            {
                res.AddError("Ошибка запроса. req.HardwareId <= 0 || req.PortNum < 0. error: {C36DDFD8-4E87-4780-9F1F-0693FBD82F00}");
                return Task.FromResult(res);
            }
            PortModelDB? port_db;
            lock (ServerContext.DbLocker)
            {
                using ServerContext db = new();
                port_db = db.Ports.FirstOrDefault(x => x.HardwareId == req.HardwareId && x.PortNum == req.PortNum);
                if (port_db is null)
                {
                    if (!req.CreatePortIfNoptExist)
                    {
                        res.AddError("Ошибка. port_db is null && !req.CreatePortIfNoptExist. error: {0CBA4187-D799-481F-BAD9-D06E3D20E068}");
                        return Task.FromResult(res);
                    }
                    port_db = new PortModelDB() { HardwareId = req.HardwareId, PortNum = req.PortNum, Name = $"№ {req.PortNum}" };
                    db.Add(port_db);
                    db.SaveChanges();
                    res.AddInfo("Порт создан");
                }
                res.Entry = new EntryModel() { Id = port_db.Id, Name = port_db.Name };
            }
            return Task.FromResult(res);
        }

        /// <inheritdoc/>
        public Task<HardwareResponseModel> HardwareUpdate(HardwareBaseModel hardware)
        {
            HardwareResponseModel res = new();

            if (string.IsNullOrWhiteSpace(hardware.Address) || string.IsNullOrWhiteSpace(hardware.Password))
            {
                res.AddError("string.IsNullOrWhiteSpace(hardware.Name) || string.IsNullOrWhiteSpace(hardware.Address) || string.IsNullOrWhiteSpace(hardware.Password). error {2417D629-D6E5-4499-80DA-F4B7AEF79F77}");
                return Task.FromResult(res);
            }

            hardware.Name = hardware.Name.Trim();

            hardware.Address = hardware.Address.Trim();
            hardware.Password = hardware.Password.Trim();

            HardwareModelDB? db_hw;
            lock (ServerContext.DbLocker)
            {
                using ServerContext db = new();

                db_hw = db.Hardwares.FirstOrDefault(x => x.Id != hardware.Id && x.Address == hardware.Address);
                if (db_hw is not null)
                {
                    res.AddError("устройство с таким адресом уже существует. error {1389FAB4-76E4-4BC9-A5B3-569CD5068388}");
                    return Task.FromResult(res);
                }

                if (hardware.Id > 0)
                {
                    db_hw = db.Hardwares.FirstOrDefault(x => x.Id == hardware.Id);
                    if (db_hw is null)
                    {
                        res.AddError("db_hw is null. error {08810FA6-75F3-4C85-90D2-F0205ED40053}");
                        return Task.FromResult(res);
                    }

                    db_hw.Name = hardware.Name;
                    db_hw.Address = hardware.Address;
                    db_hw.Password = hardware.Password;
                    db_hw.AlarmSubscriber = hardware.AlarmSubscriber;
                    db_hw.CommandsAllowed = hardware.CommandsAllowed;
                    db.Update(db_hw);
                    db.SaveChanges();
                }
                else
                {
                    db_hw = new(hardware);
                    db.Add(db_hw);
                    db.SaveChanges();
                }
            }
            res.Hardware = new(db_hw!);

            return Task.FromResult(res);
        }

        /// <inheritdoc/>
        public Task<ResponseBaseModel> SetNamePort(EntryModel port_id_name)
        {
            ResponseBaseModel res = new();
            if (port_id_name.Id < 0 || string.IsNullOrWhiteSpace(port_id_name.Name))
            {
                res.AddError("Ошибка запроса. port_id_name.Id < 0 || string.IsNullOrWhiteSpace(port_id_name.Name). error: {FCA9D7C3-10E5-4DEC-B43E-E252B88CBB77}");
                return Task.FromResult(res);
            }
            PortModelDB? port_db;
            lock (ServerContext.DbLocker)
            {
                using ServerContext db = new();
                port_db = db.Ports.FirstOrDefault(x => x.Id == port_id_name.Id);
                if (port_db is null)
                {
                    res.AddError("Порт не найден. error: {8EDBCFD6-1FDD-4D1A-B411-4D0968F60515}");
                    return Task.FromResult(res);
                }
                port_db.Name = port_id_name.Name;
                db.Update(port_db);
                db.SaveChanges();
                res.AddSuccess("Порт обновлён");
            }
            return Task.FromResult(res);
        }

        /// <inheritdoc/>
        public Task<ResponseBaseModel> HardwareDelete(int hardware_id)
        {
            ResponseBaseModel res = new();
            HardwareModelDB? hw_db;
            lock (ServerContext.DbLocker)
            {
                using ServerContext db = new();
                hw_db = db.Hardwares.FirstOrDefault(x => x.Id == hardware_id);
                if (hw_db is null)
                {
                    res.AddError("Устройство не найдено. error: {FC7CDDB7-6669-4FAE-A3B8-8793FECE83F3}");
                    return Task.FromResult(res);
                }

                db.Remove(hw_db);
                db.SaveChanges();
                res.AddSuccess("Устройство удалено");
            }
            return Task.FromResult(res);
        }
    }
}