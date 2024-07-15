////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using System.Collections.Specialized;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ab.context;
using System.Net;
using System.Web;
using MudBlazor;
using SharedLib;

namespace ServerLib;

/// <summary>
/// Устройства
/// </summary>
public class HardwiresLocalService(ILogger<HardwiresLocalService> Logger, IDbContextFactory<ServerContext> DbFactory) : IHardwiresService
{
    /// <inheritdoc/>
    public async Task<HttpResponseModel> GetHardwareHtmlPage(HardwareGetHttpRequestModel req, CancellationToken cancellation_token = default)
    {
        HardwareModelDB? db_hw;
        lock (ServerContext.DbLocker)
        {
            using ServerContext db = DbFactory.CreateDbContext();
            db_hw = db.Hardwires.FirstOrDefault(x => x.Id == req.HardwareId);
            if (db_hw is null)
                return (HttpResponseModel)ResponseBaseModel.CreateError("db.Hardwires.FirstOrDefault(x => x.Id == hardware_id) IS NULL: {98F67202-25A2-4D64-918B-72E651D63D15}");
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
            using HttpClient client = new()
            {
                BaseAddress = new Uri(db_hw.Address),
                Timeout = TimeSpan.FromSeconds(3)
            };

            string uri_path = string.IsNullOrWhiteSpace(req.Path)
                ? db_hw.Password
                : req.Path;

            if (!uri_path.StartsWith(db_hw.Password))
                uri_path = $"{db_hw.Password}/{uri_path}";

            HttpResponseMessage response = await client.GetAsync(uri_path, cancellation_token);
            return new()
            {
                StatusCode = response.StatusCode,
                Response = await response.Content.ReadAsStringAsync(cancellation_token)
            };
        }
        catch (TaskCanceledException tcex)
        {
            return new()
            {
                StatusCode = HttpStatusCode.RequestTimeout,
                Messages =
                    [
                        new ResultMessage() { TypeMessage = ResultTypesEnum.Warning, Text = $"Контроллер [{db_hw.Address}] недоступен" },
                        new ResultMessage() { TypeMessage = ResultTypesEnum.Error, Text = tcex.Message }
                    ]
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Messages = [new ResultMessage() { TypeMessage = ResultTypesEnum.Error, Text = ex.Message }]
            };
        }
    }

    /// <inheritdoc/>
    public Task<TResponseModel<HardwareBaseModel>> HardwareGet(int hardware_id, CancellationToken cancellation_token = default)
    {
        TResponseModel<HardwareBaseModel> res_hw = new();

        lock (ServerContext.DbLocker)
        {
            using ServerContext db = DbFactory.CreateDbContext();
            HardwareModelDB? db_hw = db.Hardwires.FirstOrDefault(x => x.Id == hardware_id);
            if (db_hw is null)
            {
                res_hw.AddError("Ошибка выполнения запроса: {771C7F32-36A7-4EC8-9F6C-7ED48D6FA99B}");
                return Task.FromResult(res_hw);
            }
            res_hw.Response = HardwareBaseModel.Build(db_hw);
        }
        return Task.FromResult(res_hw);
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<PortHardwareModel>> HardwarePortGet(int port_id, CancellationToken cancellation_token = default)
    {
        TResponseModel<PortHardwareModel> res_port = new();
        using ServerContext db = await DbFactory.CreateDbContextAsync(cancellation_token);
        lock (ServerContext.DbLocker)
        {
            PortModelDB? db_port = db.Ports.Include(x => x.Hardware).FirstOrDefault(x => x.Id == port_id);
            if (db_port is null)
            {
                res_port.AddError("Ошибка выполнения запроса: {5AD81739-6A9B-4753-A47C-C278CD64705B}");
                return res_port;
            }
            res_port.Response = PortHardwareModel.Build(db_port);
        }

        return res_port;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<List<HardwareModel>>> HardwiresGetAll(CancellationToken cancellation_token = default)
    {
        TResponseModel<List<HardwareModel>> res_hws = new();
        using ServerContext db = await DbFactory.CreateDbContextAsync(cancellation_token);
        lock (ServerContext.DbLocker)
        {
            res_hws.Response = db.Hardwires
                .Include(x => x.Ports)
                .ToArray()
                .Select(x => HardwareModel.Build(x))
                .ToList();
        }
        return res_hws;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<List<EntryModel>>> HardwiresGetAllAsEntries(CancellationToken cancellation_token = default)
    {
        TResponseModel<List<EntryModel>> res_hws = new();
        using ServerContext db = await DbFactory.CreateDbContextAsync(cancellation_token);
        lock (ServerContext.DbLocker)
        {
            res_hws.Response = [.. db.Hardwires.Select(x => new EntryModel() { Id = x.Id, Name = x.Name })];
        }
        return res_hws;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<List<EntryNestedModel>>> HardwiresGetTreeNestedEntries(CancellationToken cancellation_token = default)
    {
        TResponseModel<List<EntryNestedModel>> res_tree_hw = new();
        using ServerContext db = await DbFactory.CreateDbContextAsync(cancellation_token);
        lock (ServerContext.DbLocker)
        {
            res_tree_hw.Response = db.Ports.Include(x => x.Hardware)
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
                .ToList();
        }
        return res_tree_hw;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<EntryModel>> CheckPortHardware(PortHardwareCheckRequestModel req, CancellationToken cancellation_token = default)
    {
        TResponseModel<EntryModel> res = new();
        if (req.HardwareId <= 0 || req.PortNum < 0)
        {
            res.AddError("Ошибка запроса. req.HardwareId <= 0 || req.PortNum < 0. error: {C36DDFD8-4E87-4780-9F1F-0693FBD82F00}");
            return res;
        }
        PortModelDB? port_db;
        using ServerContext db = await DbFactory.CreateDbContextAsync(cancellation_token);
        lock (ServerContext.DbLocker)
        {
            port_db = db.Ports.FirstOrDefault(x => x.HardwareId == req.HardwareId && x.PortNum == req.PortNum);
            if (port_db is null)
            {
                if (!req.CreatePortIfNotExist)
                {
                    res.AddError("Ошибка. port_db is null && !req.CreatePortIfNoptExist. error: {0CBA4187-D799-481F-BAD9-D06E3D20E068}");
                    return res;
                }
                port_db = new PortModelDB() { HardwareId = req.HardwareId, PortNum = req.PortNum, Name = $"№ {req.PortNum}" };
                db.Add(port_db);
                db.SaveChanges();
                res.AddInfo($"Порт создан: №{port_db.PortNum} - '{port_db.Name}'");
            }
            res.Response = new EntryModel() { Id = port_db.Id, Name = port_db.Name };
        }
        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<HardwareBaseModel>> HardwareUpdate(HardwareBaseModel hardware, CancellationToken cancellation_token = default)
    {
        TResponseModel<HardwareBaseModel> res = new();

        if (string.IsNullOrWhiteSpace(hardware.Address) || string.IsNullOrWhiteSpace(hardware.Password))
        {
            res.AddError("string.IsNullOrWhiteSpace(hardware.Name) || string.IsNullOrWhiteSpace(hardware.Address) || string.IsNullOrWhiteSpace(hardware.Password). error {2417D629-D6E5-4499-80DA-F4B7AEF79F77}");
            return res;
        }

        hardware.Name = hardware.Name.Trim();
        hardware.Address = hardware.Address.Trim();
        hardware.Password = hardware.Password.Trim();

        HardwareModelDB? db_hw;
        using ServerContext db = await DbFactory.CreateDbContextAsync(cancellation_token);

        lock (ServerContext.DbLocker)
        {
            db_hw = db.Hardwires.Include(x => x.Ports).FirstOrDefault(x => x.Id != hardware.Id && x.Address == hardware.Address);
            if (db_hw is not null)
            {
                res.AddError("устройство с таким адресом уже существует. error {1389FAB4-76E4-4BC9-A5B3-569CD5068388}");
                return res;
            }

            if (hardware.Id > 0)
            {
                db_hw = db.Hardwires.Include(x => x.Ports).FirstOrDefault(x => x.Id == hardware.Id);
                if (db_hw is null)
                {
                    res.AddError("db_hw is null. error {08810FA6-75F3-4C85-90D2-F0205ED40053}");
                    return res;
                }

                db_hw.Name = hardware.Name;
                db_hw.Address = hardware.Address;
                db_hw.Password = hardware.Password;
                db_hw.AlarmSubscriber = hardware.AlarmSubscriber;
                db_hw.CommandsAllowed = hardware.CommandsAllowed;
                db.Update(db_hw);
                db.SaveChanges();
                res.AddSuccess("Сохранение выполнено");
            }
            else
            {
                db_hw = HardwareModelDB.Build(hardware);
                db.Add(db_hw);
                db.SaveChanges();
                res.AddSuccess("Устройство создано");
            }
        }
        res.Response = HardwareBaseModel.Build(db_hw!);

        HttpResponseModel? http_resp = null;
        HardwareGetHttpRequestModel hw_request = new() { HardwareId = db_hw.Id };
        try
        {
            http_resp = await GetHardwareHtmlPage(hw_request, cancellation_token);

            if (http_resp?.IsSuccess != true)
            {
                if (http_resp?.Messages.Count != 0 == true)
                    res.AddMessages(http_resp!.Messages);
                else
                    res.AddError("http_resp?.IsSuccess != true error {277DC035-0C62-457A-B466-805AAF9733A9}");

                return res;
            }
        }
        catch (Exception ex)
        {
            Logger.LogError("error {66A3933A-D1DA-479F-B271-90A4763A7958}", ex);
            res.AddError(ex.Message);
        }

        HtmlDomModel dom = await GetDom(http_resp?.Response ?? "");

        HtmlDomModel sub_dom;
        NameValueCollection query_parameters;

        List<uint> ports = [];
        string? _port_key;
        string? _port_num;
        string? _href;

        dom.RemoveAll(x => !x.Value!.NodeName.Equals("a", StringComparison.OrdinalIgnoreCase) || (x.Value!.NodeName.Equals("a", StringComparison.OrdinalIgnoreCase) && !x.Value!.Attributes?.Any(y => y.Key.Equals("href", StringComparison.OrdinalIgnoreCase) && y.Value?.StartsWith($"/{db_hw.Password}/?", StringComparison.OrdinalIgnoreCase) == true) == true));

        foreach (TreeItemData<HtmlDomTreeItemDataModel> link in dom)
        {
            _href = link.Value!.Attributes!.First(x => x.Key.Equals("href", StringComparison.OrdinalIgnoreCase)).Value;
            hw_request.Path = _href;
            _href = _href![(_href!.IndexOf('?') + 1)..];
            query_parameters = HttpUtility.ParseQueryString(_href);
            _port_key = query_parameters.AllKeys.FirstOrDefault(x => x?.Equals("pt", StringComparison.OrdinalIgnoreCase) == true);
            _port_num = query_parameters[_port_key];

            if (uint.TryParse(_port_num, out uint _port_num_uint) && !ports.Contains(_port_num_uint))
                ports.Add(_port_num_uint);

            try
            {
                http_resp = await GetHardwareHtmlPage(hw_request, cancellation_token);
                sub_dom = await GetDom(http_resp.Response ?? "");
                sub_dom.RemoveAll(x =>
                {
                    if (!x.Value!.NodeName.Equals("a", StringComparison.OrdinalIgnoreCase))
                        return true;

                    _href = x.Value!.Attributes?.FirstOrDefault(y => y.Key.Equals("href", StringComparison.OrdinalIgnoreCase) && y.Value?.StartsWith($"/{db_hw.Password}/?", StringComparison.OrdinalIgnoreCase) == true).Value;
                    if (string.IsNullOrWhiteSpace(_href))
                        return true;
                    _href = _href[(_href.IndexOf("?") + 1)..];
                    query_parameters = HttpUtility.ParseQueryString(_href);

                    _port_key = query_parameters.AllKeys.FirstOrDefault(x => x?.Equals("pt", StringComparison.OrdinalIgnoreCase) == true);
                    _port_num = query_parameters[_port_key];

                    if (uint.TryParse(_port_num, out uint _port_num_uint) && !ports.Contains(_port_num_uint))
                        ports.Add(_port_num_uint);

                    return !query_parameters.AllKeys.Any(x => x?.Equals("pt", StringComparison.OrdinalIgnoreCase) == true);
                });
            }
            catch (Exception ex)
            {
                Logger.LogError("error {7E47C193-E41F-4C49-BC6F-2DAD953198FC}", ex);
                res.AddError(ex.Message);
            }
        }

        List<PortModelDB> _ports_bd = db_hw.Ports!.Where(x => x.HardwareId == db_hw.Id && !ports.Contains(x.PortNum)).ToList();
        if (_ports_bd.Count != 0)
        {
            res.AddError($"В базе данных обнаружены порты с несуществующими номерами: {string.Join(", ", _ports_bd.Select(x => x.PortNum))}");
            lock (ServerContext.DbLocker)
            {
                _ports_bd.ForEach(x => x.IsDisable = true);
                db.UpdateRange(_ports_bd);
                db.SaveChanges();
            }
        }

        _ports_bd = ports.Where(x => !db_hw.Ports!.Any(y => y.PortNum == x)).Select(x => new PortModelDB() { HardwareId = db_hw.Id, PortNum = x }).ToList();
        if (_ports_bd.Count != 0)
        {
            res.AddInfo($"В базу данных добавлены порты с новыми номерами: {string.Join(", ", _ports_bd.Select(x => x.PortNum))}");
            lock (ServerContext.DbLocker)
            {
                db.AddRange(_ports_bd);
                db.SaveChanges();
            }
        }

        return res;
    }



    /// <summary>
    /// 
    /// </summary>
    public async Task<HtmlDomModel> GetDom(string raw)
    {
        HtmlDomModel res = [];
        await res.Reload(raw ?? "");
        return res;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetNamePort(EntryModel port_id_name, CancellationToken cancellation_token = default)
    {
        ResponseBaseModel res = new();
        if (port_id_name.Id < 0 || string.IsNullOrWhiteSpace(port_id_name.Name))
        {
            res.AddError("Ошибка запроса. port_id_name.Id < 0 || string.IsNullOrWhiteSpace(port_id_name.Name). error: {FCA9D7C3-10E5-4DEC-B43E-E252B88CBB77}");
            return res;
        }
        PortModelDB? port_db;
        using ServerContext db = await DbFactory.CreateDbContextAsync();
        lock (ServerContext.DbLocker)
        {
            port_db = db.Ports.FirstOrDefault(x => x.Id == port_id_name.Id);
            if (port_db is null)
            {
                res.AddError("Порт не найден. error: {8EDBCFD6-1FDD-4D1A-B411-4D0968F60515}");
                return res;
            }
            port_db.Name = port_id_name.Name;
            db.Update(port_db);
            db.SaveChanges();
            res.AddSuccess("Порт обновлён");
        }
        return res;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> HardwareDelete(int hardware_id, CancellationToken cancellation_token = default)
    {
        ResponseBaseModel res = new();
        HardwareModelDB? hw_db;
        using ServerContext db = await DbFactory.CreateDbContextAsync(cancellation_token);
        lock (ServerContext.DbLocker)
        {
            hw_db = db.Hardwires.FirstOrDefault(x => x.Id == hardware_id);
            if (hw_db is null)
            {
                res.AddError("Устройство не найдено. error: {FC7CDDB7-6669-4FAE-A3B8-8793FECE83F3}");
                return res;
            }

            db.Remove(hw_db);
            db.SaveChanges();
            res.AddSuccess("Устройство удалено");
        }
        return res;
    }
}