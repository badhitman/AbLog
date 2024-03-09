////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Newtonsoft.Json;
using SharedLib;

namespace ServerLib;

/// <summary>
/// Команды порта
/// </summary>
public class CommandsMqttService(IMqttBaseService mqtt, MqttConfigModel mqtt_conf) : ICommandsService
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> CommandDelete(int id_command, CancellationToken cancellation_token = default)
    {
        ResponseBaseModel res = new();
        SimpleStringResponseModel rpc = await mqtt.MqttRemoteCall(new LongIdNoiseModel() { Id = id_command }, $"{mqtt_conf.PrefixMqtt}{GlobalStatic.Routes.Commands}/{GlobalStatic.Routes.DELETE}", cancellation_token);

        if (!rpc.IsSuccess)
        {
            res.AddMessages(rpc.Messages);
            return res;
        }
        if (string.IsNullOrEmpty(rpc.Response))
        {
            res.AddError("string.IsNullOrEmpty(rpc.Response). error {5313CC95-B5F0-4C88-BAF5-DB53AA9A670F}");
            return res;
        }
        ResponseBaseModel? response_mqtt = JsonConvert.DeserializeObject<ResponseBaseModel>(rpc.Response);

        if (response_mqtt is null)
            res.AddError("response_mqtt is null. error {87F7F551-A9EE-406B-8DD6-364B603C35F4}");
        else
            return response_mqtt;

        return res;
    }

    /// <inheritdoc/>
    public async Task<CommandResponseModel> CommandGet(int command_id, CancellationToken cancellation_token = default)
    {
        CommandResponseModel res = new();

        SimpleStringResponseModel rpc = await mqtt.MqttRemoteCall(new LongIdNoiseModel() { Id = command_id }, $"{mqtt_conf.PrefixMqtt}{GlobalStatic.Routes.Commands}/{GlobalStatic.Routes.GET}", cancellation_token);

        if (!rpc.IsSuccess)
        {
            res.AddMessages(rpc.Messages);
            return res;
        }

        if (string.IsNullOrEmpty(rpc.Response))
        {
            res.AddError("string.IsNullOrEmpty(rpc.Response). error {C23EC21C-FB5D-4F2C-B172-28B3A070A88D}");
            return res;
        }

        CommandResponseModel? response_mqtt = JsonConvert.DeserializeObject<CommandResponseModel>(rpc.Response);

        if (response_mqtt is null)
            res.AddError("response_mqtt is null. error {216D0026-3D59-4F32-9B2D-94C18ACB0475}");
        else
            return response_mqtt;

        return res;
    }

    /// <inheritdoc/>
    public async Task<EntriesSortingResponseModel> CommandSortingSet(IdsPairModel req, CancellationToken cancellation_token = default)
    {
        EntriesSortingResponseModel res = new();
        SimpleStringResponseModel rpc = await mqtt.MqttRemoteCall(req, $"{mqtt_conf.PrefixMqtt}{GlobalStatic.Routes.Commands}/{GlobalStatic.Routes.SORTING}", cancellation_token);

        if (!rpc.IsSuccess)
        {
            res.AddMessages(rpc.Messages);
            return res;
        }
        if (string.IsNullOrEmpty(rpc.Response))
        {
            res.AddError("string.IsNullOrEmpty(rpc.Response). error {F4A71303-B960-4D13-A7D0-42E80E2684CD}");
            return res;
        }
        EntriesSortingResponseModel? response_mqtt = JsonConvert.DeserializeObject<EntriesSortingResponseModel>(rpc.Response);

        if (response_mqtt is null)
            res.AddError("response_mqtt is null. error {180C3E81-29AB-4BC0-B138-BE3BBB3FB4CC}");
        else
            return response_mqtt;

        return res;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> CommandUpdateOrCreate(CommandModelDB req, CancellationToken cancellation_token = default)
    {
        ResponseBaseModel res = new();
        SimpleStringResponseModel rpc = await mqtt.MqttRemoteCall(req, $"{mqtt_conf.PrefixMqtt}{GlobalStatic.Routes.Commands}/{GlobalStatic.Routes.UPDATE}", cancellation_token);

        if (!rpc.IsSuccess)
        {
            res.AddMessages(rpc.Messages);
            return res;
        }
        if (string.IsNullOrEmpty(rpc.Response))
        {
            res.AddError("string.IsNullOrEmpty(rpc.Response). error {67B3F6B9-91C5-4F6B-BF09-0625127DD7BC}");
            return res;
        }
        ResponseBaseModel? response_mqtt = JsonConvert.DeserializeObject<ResponseBaseModel>(rpc.Response);

        if (response_mqtt is null)
            res.AddError("response_mqtt is null. error {B413974C-8330-4375-A86C-A3F9A1446B1D}");
        else
            return response_mqtt;

        return res;
    }

    /// <inheritdoc/>
    public async Task<EntriesSortingResponseModel> GetCommandsEntriesByScript(int script_id, CancellationToken cancellation_token = default)
    {
        EntriesSortingResponseModel res = new();
        SimpleStringResponseModel rpc = await mqtt.MqttRemoteCall(new LongIdNoiseModel() { Id = script_id }, $"{mqtt_conf.PrefixMqtt}{GlobalStatic.Routes.Commands}/{GlobalStatic.Routes.ENTRIES}/{GlobalStatic.Routes.BY_OWNER}", cancellation_token);

        if (!rpc.IsSuccess)
        {
            res.AddMessages(rpc.Messages);
            return res;
        }
        if (string.IsNullOrEmpty(rpc.Response))
        {
            res.AddError("string.IsNullOrEmpty(rpc.Response). error {B04C831D-0B07-414B-AB35-B3ED475D8438}");
            return res;
        }
        EntriesSortingResponseModel? response_mqtt = JsonConvert.DeserializeObject<EntriesSortingResponseModel>(rpc.Response);

        if (response_mqtt is null)
            res.AddError("response_mqtt is null. error {FF443B00-6625-4925-906A-156151D81FFA}");
        else
            return response_mqtt;

        return res;
    }
}