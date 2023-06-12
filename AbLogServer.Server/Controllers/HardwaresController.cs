using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ab.context;
using SharedLib;

namespace AbLogServer.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class HardwaresController : ControllerBase
    {
        private readonly ILogger<HardwaresController> _logger;

        /// <summary>
        /// 
        /// </summary>
        public HardwaresController(ILogger<HardwaresController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        [HttpGet($"{GlobalStatic.HttpRoutes.LIST}")]
        public HardwaresResponseModel HardwaresGetAll()
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
            return res_hws;
        }

        /// <summary>
        /// 
        /// </summary>
        [HttpGet($"{GlobalStatic.HttpRoutes.ENTRIES}")]
        public EntriesResponseModel HardwaresGetAllAsEntries()
        {
            EntriesResponseModel res_hws = new();
            lock (ServerContext.DbLocker)
            {
                using ServerContext db = new();
                res_hws.Entries = db.Hardwares.Select(x => new EntryModel() { Id = x.Id, Name = x.Name }).ToArray();
            }
            return res_hws;
        }

        /// <summary>
        /// 
        /// </summary>
        [HttpGet($"{GlobalStatic.HttpRoutes.NESTED_ENTRIES}")]
        public EntriesNestedResponseModel HardwaresGetTreeNestedEntries()
        {
            EntriesNestedResponseModel res_tree_hw = new();
            lock (ServerContext.DbLocker)
            {
                using ServerContext db = new();

                res_tree_hw.Entries = db.Ports.Include(x => x.Hardware)
                    .Select(x => new { port_id = x.Id, port_num = x.PortNumb, hw_id = x.HardwareId, hw_name = x.Hardware!.Name, hw_address = x.Hardware.Address })
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
            return res_tree_hw;
        }

        /// <summary>
        /// 
        /// </summary>
        [HttpGet("{hardware_id}")]
        public HardwareResponseModel HardwareGet(int hardware_id)
        {
            HardwareResponseModel res_hw = new();

            lock (ServerContext.DbLocker)
            {
                using ServerContext db = new();
                HardwareModelDB? db_hw = db.Hardwares.Include(x => x.Ports).FirstOrDefault(x => x.Id == hardware_id);
                if (db_hw is null)
                {
                    res_hw.AddError("Ошибка выполнения запроса: {771C7F32-36A7-4EC8-9F6C-7ED48D6FA99B}");
                    return res_hw;
                }
                res_hw.Hardware = new(db_hw);
            }
            return res_hw;
        }

        /// <summary>
        /// 
        /// </summary>
        [HttpGet($"{GlobalStatic.HttpRoutes.Ports}/{{port_id}}")]
        public PortHardwareResponseModel HardwarePortGet(int port_id)
        {
            PortHardwareResponseModel res_port = new();
            lock (ServerContext.DbLocker)
            {
                using ServerContext db = new();
                PortModelDB? db_port = db.Ports.Include(x => x.Hardware).FirstOrDefault(x => x.Id == port_id);
                if (db_port is null)
                {
                    res_port.AddError("Ошибка выполнения запроса: {5AD81739-6A9B-4753-A47C-C278CD64705B}");
                    return res_port;
                }
                res_port.Port = new PortHardwareModel(db_port);
            }

            return res_port;
        }
    }
}