using Microsoft.AspNetCore.Mvc;
using SharedLib.IServices;
using SharedLib;

namespace AbLogServer.Controllers
{
    /// <summary>
    /// ����������� �����
    /// </summary>
    [ApiController]
    [Route("/api/[controller]")]
    public class HardwaresController : ControllerBase
    {
        readonly ILogger<HardwaresController> _logger;
        readonly IHardwaresService _hardwares_service;

        /// <summary>
        /// ����������� �����
        /// </summary>
        public HardwaresController(ILogger<HardwaresController> logger, IHardwaresService hardwares_service)
        {
            _logger = logger;
            _hardwares_service = hardwares_service;
        }

        /// <summary>
        /// ���������� (���)
        /// </summary>
        [HttpGet($"{GlobalStatic.HttpRoutes.LIST}")]
        public async Task<HardwaresResponseModel> HardwaresGetAll() => await _hardwares_service.HardwaresGetAll();

        /// <summary>
        /// ���������� (���) � ���� Entries[]
        /// </summary>
        [HttpGet($"{GlobalStatic.HttpRoutes.ENTRIES}")]
        public async Task<EntriesResponseModel> HardwaresGetAllAsEntries() => await _hardwares_service.HardwaresGetAllAsEntries();

        /// <summary>
        /// ���������� (���) � ���� Entries[] ������ � �������
        /// </summary>
        [HttpGet($"{GlobalStatic.HttpRoutes.NESTED_ENTRIES}")]
        public async Task<EntriesNestedResponseModel> HardwaresGetTreeNestedEntries() => await _hardwares_service.HardwaresGetTreeNestedEntries();

        /// <summary>
        /// ���������� (����������� ����/����������)
        /// </summary>
        [HttpGet("{hardware_id}")]
        public async Task<HardwareResponseModel> HardwareGet(int hardware_id) => await _hardwares_service.HardwareGet(hardware_id);

        /// <summary>
        /// ���� �����������
        /// </summary>
        [HttpGet($"{GlobalStatic.HttpRoutes.Ports}/{{port_id}}")]
        public async Task<PortHardwareResponseModel> HardwarePortGet(int port_id) => await _hardwares_service.HardwarePortGet(port_id);

        /// <summary>
        /// �������� HTML �������� �������� ����������
        /// </summary>
        [HttpPost($"{GlobalStatic.HttpRoutes.HTML}/{GlobalStatic.HttpRoutes.MAIN}")]
        public async Task<HttpResponseModel> GetHardwareHtmlPage(HardvareGetRequestModel req) => await _hardwares_service.GetHardwareHtmlPage(req);
    }
}