////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using BlazorLib;
using Refit;
using SharedLib;

namespace RazorLib;

/// <summary>
/// Tools
/// </summary>
public class ToolsRefitService : IToolsService
{
    readonly IRefitToolsService _refit_tools;

    /// <summary>
    /// Tools
    /// </summary>
    public ToolsRefitService(IRefitToolsService refit_tools)
    {
        _refit_tools = refit_tools;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> StartMqtt()
    {
        Refit.ApiResponse<ResponseBaseModel> rest = await _refit_tools.StartMqtt();
        if (rest.Content is null || !rest.IsSuccessStatusCode)
            return ResponseBaseModel.CreateError($"rest.Content is null || !rest.IsSuccessStatusCode. error {{CBDF8008-D059-4584-B1FF-901B24311186}}");

        return rest.Content;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> StopMqtt()
    {
        Refit.ApiResponse<ResponseBaseModel> rest = await _refit_tools.StopMqtt();
        if (rest.Content is null || !rest.IsSuccessStatusCode)
            return ResponseBaseModel.CreateError($"rest.Content is null || !rest.IsSuccessStatusCode. error {{7702500C-AAE6-4C24-9EEC-CD63BC7651A1}}");

        return rest.Content;
    }

    /// <inheritdoc/>
    public async Task<BoolResponseModel> StatusMqtt()
    {
        Refit.ApiResponse<BoolResponseModel> rest = await _refit_tools.StatusMqtt();
        if (rest.Content is null || !rest.IsSuccessStatusCode)
        {
            BoolResponseModel res = new();
            res.AddError($"rest.Content is null || !rest.IsSuccessStatusCode. error {{A1B10B71-A1D3-412E-B78F-690D1EABB575}}");
            return res;
        }

        return rest.Content;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> TestMqttConnect(MqttConfigModel? conf = null)
    {
        ResponseBaseModel res = new();
        Refit.ApiResponse<ResponseBaseModel> rest = await _refit_tools.MqttConfigTestSmtpConnection(conf);
        if (rest.Content is null || !rest.IsSuccessStatusCode)
            res.AddError($"rest.Content is null || !rest.IsSuccessStatusCode {{3657D4F9-E721-4172-AC9E-82A49D04198D}}");
        if (rest.Error is not null)
            res.AddError(rest.Error.Message);

        if (!res.IsSuccess)
            return res;

        return rest.Content!;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> TestEmailConnect(EmailConfigModel? conf = null)
    {
        ResponseBaseModel res = new();
        Refit.ApiResponse<ResponseBaseModel> rest = await _refit_tools.EmailConfigTestSmtpConnection(conf);
        if (rest.Content is null || !rest.IsSuccessStatusCode)
            res.AddError($"rest.Content is null || !rest.IsSuccessStatusCode {{BC0D377C-03BF-42FB-B5DF-3667B98FB7F2}}");

        if (rest.Error is not null)
            res.AddError(rest.Error.Message);

        if (!res.IsSuccess)
            return res;

        return rest.Content!;
    }

    /// <inheritdoc/>
    public async Task<MqttPublishMessageResultModel> PublishMqttMessage(MqttPublishMessageModel message)
    {
        MqttPublishMessageResultModel res = new();

        Refit.ApiResponse<MqttPublishMessageResultModel> rest = await _refit_tools.PublishMqttMessage(message);
        if (rest.Content is null || !rest.IsSuccessStatusCode)
            res.AddError($"rest.Content is null || !rest.IsSuccessStatusCode {{FA1F8616-B93B-445F-B92B-A37FE4F24C7F}}");

        if (rest.Error is not null)
            res.AddError(rest.Error.Message);

        if (!res.IsSuccess)
            return res;

        return rest.Content!;
    }

    /// <inheritdoc/>
    public virtual async Task<DictionaryResponseModel> TestTelegramBotConnect(TelegramBotConfigModel? conf = null)
    {
        DictionaryResponseModel res = new();
        ApiResponse<DictionaryResponseModel> rest = await _refit_tools.TelegramBotConfigTest(conf);
                
        if (rest.Content is null || !rest.IsSuccessStatusCode)
            res.AddError($"rest.Content is null || !rest.IsSuccessStatusCode {{FA1F8616-B93B-445F-B92B-A37FE4F24C7F}}");

        if (rest.Error is not null)
            res.AddError(rest.Error.Message);

        if (!res.IsSuccess)
            return res;

        return rest.Content!;
    }
}