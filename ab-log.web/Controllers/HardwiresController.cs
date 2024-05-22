////////////////////////////////////////////////
// � https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Mvc;
using SharedLib;

namespace ABLogWeb;

/// <summary>
/// ����������� �����
/// </summary>
[Route("/api/[controller]"), ApiController]
public class HardwiresController(IHardwiresService hardwires_service) : ControllerBase
{
    /// <summary>
    /// ���������� (���)
    /// </summary>
    [HttpGet($"/{GlobalStatic.Routes.Hardwires}/{GlobalStatic.Routes.LIST}")]
    public async Task<TResponseModel<List<HardwareModel>>> HardwiresGetAll(CancellationToken cancellation_token = default)
        => await hardwires_service.HardwiresGetAll(cancellation_token);

    /// <summary>
    /// ���������� (���) � ���� Entries[]
    /// </summary>
    [HttpGet($"/{GlobalStatic.Routes.Hardwires}/{GlobalStatic.Routes.ENTRIES}")]
    public async Task<TResponseModel<List<EntryModel>>> HardwiresGetAllAsEntries(CancellationToken cancellation_token = default)
        => await hardwires_service.HardwiresGetAllAsEntries(cancellation_token);

    /// <summary>
    /// ���������� (���) � ���� Entries[] ������ � �������
    /// </summary>
    [HttpGet($"/{GlobalStatic.Routes.Hardwires}/{GlobalStatic.Routes.NESTED_ENTRIES}")]
    public async Task<TResponseModel<List<EntryNestedModel>>> HardwiresGetTreeNestedEntries(CancellationToken cancellation_token = default)
        => await hardwires_service.HardwiresGetTreeNestedEntries(cancellation_token);

    /// <summary>
    /// ���������� (����������� ����/����������)
    /// </summary>
    [HttpGet($"/{GlobalStatic.Routes.Hardwires}/{GlobalStatic.Routes.GET}/{{hardware_id}}")]
    public async Task<TResponseModel<HardwareBaseModel>> HardwareGet([FromRoute] int hardware_id, CancellationToken cancellation_token = default)
        => await hardwires_service.HardwareGet(hardware_id, cancellation_token);

    /// <summary>
    /// ������� ����������
    /// </summary>
    [HttpDelete($"/{GlobalStatic.Routes.Hardwires}/{GlobalStatic.Routes.DELETE}/{{hardware_id}}")]
    public async Task<ResponseBaseModel> HardwareDelete([FromRoute] int hardware_id, CancellationToken cancellation_token = default)
        => await hardwires_service.HardwareDelete(hardware_id, cancellation_token);

    /// <summary>
    /// ��������/������� ���������� (����������� ����/����������)
    /// </summary>
    [HttpPost($"/{GlobalStatic.Routes.Hardwires}/{GlobalStatic.Routes.UPDATE}")]
    public async Task<TResponseModel<HardwareBaseModel>> HardwareUpdate(HardwareBaseModel hardware, CancellationToken cancellation_token = default)
        => await hardwires_service.HardwareUpdate(hardware, cancellation_token);

    /// <summary>
    /// ���� �����������
    /// </summary>
    [HttpGet($"/{GlobalStatic.Routes.Ports}/{{port_id}}")]
    public async Task<TResponseModel<PortHardwareModel>> HardwarePortGet([FromRoute] int port_id, CancellationToken cancellation_token = default)
        => await hardwires_service.HardwarePortGet(port_id, cancellation_token);

    /// <summary>
    /// �������� HTML �������� �������� ����������
    /// </summary>
    [HttpPost($"/{GlobalStatic.Routes.Hardwires}/{GlobalStatic.Routes.MAIN}-{GlobalStatic.Routes.HTML}")]
    public async Task<HttpResponseModel> GetHardwareHtmlPage(HardwareGetHttpRequestModel req, CancellationToken cancellation_token = default)
        => await hardwires_service.GetHardwareHtmlPage(req, cancellation_token);

    /// <summary>
    /// ������/�������� ����� ����������
    /// </summary>
    [HttpPost($"/{GlobalStatic.Routes.Ports}/{GlobalStatic.Routes.CHECK}")]
    public async Task<TResponseModel<EntryModel>> CheckPortHardware(PortHardwareCheckRequestModel req, CancellationToken cancellation_token = default)
        => await hardwires_service.CheckPortHardware(req, cancellation_token);

    /// <summary>
    /// ���������� ��� �����
    /// </summary>
    [HttpPost($"{GlobalStatic.Routes.Ports}/{GlobalStatic.Routes.UPDATE}")]
    public async Task<ResponseBaseModel> SetNamePort(EntryModel port_id_name, CancellationToken cancellation_token = default)
        => await hardwires_service.SetNamePort(port_id_name, cancellation_token);
}