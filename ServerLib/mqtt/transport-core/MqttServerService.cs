////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.Extensions.Logging;
using System.Runtime.Versioning;
using Newtonsoft.Json;
using MQTTnet.Client;
using System.Text;
using SharedLib;
using MQTTnet;
using ab.context;
using Telegram.Bot;
using Microsoft.Extensions.DependencyInjection;

namespace ServerLib;

/// <summary>
/// 
/// </summary>
[SupportedOSPlatform("windows")]
[SupportedOSPlatform("linux")]
[SupportedOSPlatform("android")]
public class MqttServerService : MqttBaseServiceAbstraction
{
    readonly IHardwaresService _hardwares_service;
    readonly ISystemCommandsService _sys_com_service;
    readonly IUsersService _users_service;
    readonly HttpClient _http_client;
    readonly IServiceCollection _services;

    /// <inheritdoc/>
    public override MqttClientSubscribeOptions MqttSubscribeOptions => _mqttFactory
                .CreateSubscribeOptionsBuilder()
                .WithTopicFilter(f => { f.WithTopic(GlobalStatic.Routes.AB_LOG_SYSTEM); })
                .WithTopicFilter(f => { f.WithTopic(GlobalStatic.Routes.Cameras); })
                .WithTopicFilter(f => { f.WithTopic(GlobalStatic.Routes.SHOT); })

                .WithTopicFilter(f => { f.WithTopic($"{GlobalStatic.Routes.Hardwares}/{GlobalStatic.Routes.HTTP}"); })
                //
                .WithTopicFilter(f => { f.WithTopic($"{GlobalStatic.Routes.Hardwares}/{GlobalStatic.Routes.LIST}"); })
                .WithTopicFilter(f => { f.WithTopic($"{GlobalStatic.Routes.Hardware}/{GlobalStatic.Routes.GET}"); })
                .WithTopicFilter(f => { f.WithTopic($"{GlobalStatic.Routes.Hardware}/{GlobalStatic.Routes.ENTRIES}"); })
                .WithTopicFilter(f => { f.WithTopic($"{GlobalStatic.Routes.Hardware}/{GlobalStatic.Routes.NESTED_ENTRIES}"); })
                .WithTopicFilter(f => { f.WithTopic($"{GlobalStatic.Routes.Hardware}/{GlobalStatic.Routes.UPDATE}"); })
                .WithTopicFilter(f => { f.WithTopic($"{GlobalStatic.Routes.Hardware}/{GlobalStatic.Routes.DELETE}"); })

                .WithTopicFilter(f => { f.WithTopic($"{GlobalStatic.Routes.Port}/{GlobalStatic.Routes.GET}"); })
                .WithTopicFilter(f => { f.WithTopic($"{GlobalStatic.Routes.Port}/{GlobalStatic.Routes.CHECK}"); })
                .WithTopicFilter(f => { f.WithTopic($"{GlobalStatic.Routes.Port}/{GlobalStatic.Routes.UPDATE}"); })

                .WithTopicFilter(f => { f.WithTopic($"{GlobalStatic.Routes.Storage}/{GlobalStatic.Routes.TelegramBot}/{GlobalStatic.Routes.GET}"); })
                .WithTopicFilter(f => { f.WithTopic($"{GlobalStatic.Routes.Storage}/{GlobalStatic.Routes.TelegramBot}/{GlobalStatic.Routes.CHECK}"); })
                .WithTopicFilter(f => { f.WithTopic($"{GlobalStatic.Routes.Storage}/{GlobalStatic.Routes.TelegramBot}/{GlobalStatic.Routes.UPDATE}"); })

                .WithTopicFilter(f => { f.WithTopic($"{GlobalStatic.Routes.Users}/{GlobalStatic.Routes.GET}/{GlobalStatic.Routes.LIST}"); })
                .WithTopicFilter(f => { f.WithTopic($"{GlobalStatic.Routes.Users}/{GlobalStatic.Routes.GET}"); })
                .WithTopicFilter(f => { f.WithTopic($"{GlobalStatic.Routes.Users}/{GlobalStatic.Routes.UPDATE}"); })

                .WithTopicFilter(f => { f.WithTopic($"{GlobalStatic.Routes.System}/{GlobalStatic.Routes.Commands}/{GlobalStatic.Routes.LIST}"); })
                .WithTopicFilter(f => { f.WithTopic($"{GlobalStatic.Routes.System}/{GlobalStatic.Routes.Commands}/{GlobalStatic.Routes.DELETE}"); })
                .WithTopicFilter(f => { f.WithTopic($"{GlobalStatic.Routes.System}/{GlobalStatic.Routes.Commands}/{GlobalStatic.Routes.UPDATE}"); })
                .WithTopicFilter(f => { f.WithTopic($"{GlobalStatic.Routes.System}/{GlobalStatic.Routes.Commands}/{GlobalStatic.Routes.START}"); })
            .Build();

    /// <summary>
    /// 
    /// </summary>
    public MqttServerService(IMqttClient mqttClient, IServiceCollection services, ISystemCommandsService sys_com_service, IUsersService users_service, HttpClient http_client, ILogger<MqttServerService> logger, MqttConfigModel mqtt_settings, MqttFactory mqttFactory, IHardwaresService hardwares_service, CancellationToken cancellation_token = default)
        : base(mqttClient, mqtt_settings, mqttFactory, logger, cancellation_token)
    {
        _logger = logger;
        _hardwares_service = hardwares_service;
        _http_client = http_client;
        _services = services;
        _sys_com_service = sys_com_service;
        _users_service = users_service;
    }

    /// <summary>
    /// 
    /// </summary>
    public override async Task ApplicationMessageReceiveHandledAsync(MqttApplicationMessageReceivedEventArgs e)
    {
        await base.ApplicationMessageReceiveHandledAsync(e);
        _logger.LogInformation($"call >> {nameof(ApplicationMessageReceiveHandledAsync)}");
        byte[] payload_bytes = await CipherService.DecryptAsync(e.ApplicationMessage.PayloadSegment.ToArray(), _mqtt_settings.Secret ?? CipherService.DefaultSecret, e.ApplicationMessage.CorrelationData);

        string payload_json = Encoding.UTF8.GetString(payload_bytes);
        string salt = Guid.NewGuid().ToString();

        switch (e.ApplicationMessage.Topic)
        {
            case $"{GlobalStatic.Routes.Hardwares}/{GlobalStatic.Routes.HTTP}":
                HardwareGetHttpRequestModel? req_http = JsonConvert.DeserializeObject<HardwareGetHttpRequestModel>(payload_json);
                if (req_http is null)
                {
                    _logger.LogError("req is null. error 1EB8300D-9436-44B0-9413-31BAA01018F2");
                    await PublishMessage(JsonConvert.SerializeObject(ResponseBaseModel.CreateError("req is null. error 1EB8300D-9436-44B0-9413-31BAA01018F2")), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);
                    break;
                }

                await PublishMessage(JsonConvert.SerializeObject(await _hardwares_service.GetHardwareHtmlPage(req_http)), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);
                break;

            case $"{GlobalStatic.Routes.Hardwares}/{GlobalStatic.Routes.LIST}":
                NoiseModel? req_noise = JsonConvert.DeserializeObject<NoiseModel>(payload_json);
                if (req_noise is null)
                {
                    _logger.LogError("req is null. error 1C55124E-8DF4-4CCC-A9FF-8CC2C0AF6B65");
                    await PublishMessage(JsonConvert.SerializeObject(ResponseBaseModel.CreateError("req is null. error 1C55124E-8DF4-4CCC-A9FF-8CC2C0AF6B65")), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);
                    break;
                }

                await PublishMessage(JsonConvert.SerializeObject(await _hardwares_service.HardwaresGetAll()), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);
                break;
            case $"{GlobalStatic.Routes.Hardwares}/{GlobalStatic.Routes.ENTRIES}":
                req_noise = JsonConvert.DeserializeObject<NoiseModel>(payload_json);
                if (req_noise is null)
                {
                    _logger.LogError("req is null. error C1595114-C3E5-4AE1-937B-8B45C59C8507");
                    await PublishMessage(JsonConvert.SerializeObject(ResponseBaseModel.CreateError("req is null. error C1595114-C3E5-4AE1-937B-8B45C59C8507")), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);
                    break;
                }

                await PublishMessage(JsonConvert.SerializeObject(await _hardwares_service.HardwaresGetAllAsEntries()), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);
                break;
            case $"{GlobalStatic.Routes.Hardwares}/{GlobalStatic.Routes.NESTED_ENTRIES}":
                req_noise = JsonConvert.DeserializeObject<NoiseModel>(payload_json);
                if (req_noise is null)
                {
                    _logger.LogError("req is null. error 787DE141-F012-4369-AD88-95ACBB7BD937");
                    await PublishMessage(JsonConvert.SerializeObject(ResponseBaseModel.CreateError("req is null. error 787DE141-F012-4369-AD88-95ACBB7BD937")), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);
                    break;
                }

                await PublishMessage(JsonConvert.SerializeObject(await _hardwares_service.HardwaresGetTreeNestedEntries()), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);
                break;
            case $"{GlobalStatic.Routes.Hardware}/{GlobalStatic.Routes.GET}":
                SimpleIdNoiseModel? req_nid = JsonConvert.DeserializeObject<SimpleIdNoiseModel>(payload_json);
                if (req_nid is null)
                {
                    _logger.LogError("req is null. error 98C12EAC-73A9-4946-8850-7B075613833E");
                    await PublishMessage(JsonConvert.SerializeObject(ResponseBaseModel.CreateError("req is null. error 98C12EAC-73A9-4946-8850-7B075613833E")), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);
                    break;
                }

                await PublishMessage(JsonConvert.SerializeObject(await _hardwares_service.HardwareGet(req_nid.Id)), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);
                break;
            case $"{GlobalStatic.Routes.Hardware}/{GlobalStatic.Routes.UPDATE}":
                HardwareBaseModel? req_hw = JsonConvert.DeserializeObject<HardwareBaseModel>(payload_json);
                if (req_hw is null)
                {
                    _logger.LogError("req is null. error F7657C6A-2601-49BE-8F65-F302F5B1B4A3");
                    await PublishMessage(JsonConvert.SerializeObject(ResponseBaseModel.CreateError("req is null. error F7657C6A-2601-49BE-8F65-F302F5B1B4A3")), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);
                    break;
                }

                await PublishMessage(JsonConvert.SerializeObject(await _hardwares_service.HardwareUpdate(req_hw)), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);
                break;
            case $"{GlobalStatic.Routes.Hardware}/{GlobalStatic.Routes.DELETE}":
                req_nid = JsonConvert.DeserializeObject<SimpleIdNoiseModel>(payload_json);
                if (req_nid is null)
                {
                    _logger.LogError("req is null. error 86375673-BAA9-41EA-83F9-0F30DB918D05");
                    await PublishMessage(JsonConvert.SerializeObject(ResponseBaseModel.CreateError("req is null. error 86375673-BAA9-41EA-83F9-0F30DB918D05")), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);
                    break;
                }

                await PublishMessage(JsonConvert.SerializeObject(await _hardwares_service.HardwareDelete(req_nid.Id)), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);
                break;

            case $"{GlobalStatic.Routes.Port}/{GlobalStatic.Routes.GET}":
                req_nid = JsonConvert.DeserializeObject<SimpleIdNoiseModel>(payload_json);
                if (req_nid is null)
                {
                    _logger.LogError("req is null. error D4D48AE6-C24E-4532-AA92-07ECB1AFB549");
                    await PublishMessage(JsonConvert.SerializeObject(ResponseBaseModel.CreateError("req is null. error D4D48AE6-C24E-4532-AA92-07ECB1AFB549")), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);
                    break;
                }

                await PublishMessage(JsonConvert.SerializeObject(await _hardwares_service.HardwarePortGet(req_nid.Id)), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);
                break;
            case $"{GlobalStatic.Routes.Port}/{GlobalStatic.Routes.CHECK}":
                PortHardwareCheckRequestModel? req_port_check = JsonConvert.DeserializeObject<PortHardwareCheckRequestModel>(payload_json);
                if (req_port_check is null)
                {
                    _logger.LogError("req is null. error 2CB7CA8A-18B3-4FDF-84F0-CD8BBEA4BA45");
                    await PublishMessage(JsonConvert.SerializeObject(ResponseBaseModel.CreateError("req is null. error 2CB7CA8A-18B3-4FDF-84F0-CD8BBEA4BA45")), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);
                    return;
                }

                await PublishMessage(JsonConvert.SerializeObject(await _hardwares_service.CheckPortHardware(req_port_check)), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);
                break;
            case $"{GlobalStatic.Routes.Port}/{GlobalStatic.Routes.UPDATE}":
                EntryModel? req_e = JsonConvert.DeserializeObject<EntryModel>(payload_json);
                if (req_e is null)
                {
                    _logger.LogError("req is null. 2415B593-DAD5-4B61-849F-7B95A174739F");
                    await PublishMessage(JsonConvert.SerializeObject(ResponseBaseModel.CreateError("req is null. 2415B593-DAD5-4B61-849F-7B95A174739F")), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);
                    break;
                }

                await PublishMessage(JsonConvert.SerializeObject(await _hardwares_service.SetNamePort(req_e)), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);
                break;

            case $"{GlobalStatic.Routes.Storage}/{GlobalStatic.Routes.TelegramBot}/{GlobalStatic.Routes.UPDATE}":
                TelegramBotConfigModel? conf_tbot = JsonConvert.DeserializeObject<TelegramBotConfigModel>(payload_json);
                if (conf_tbot is null)
                {
                    _logger.LogError("req is null. 720D6445-7D3F-4BE7-AF9E-0C57D48DB5AC");
                    await PublishMessage(JsonConvert.SerializeObject(ResponseBaseModel.CreateError("req is null. 720D6445-7D3F-4BE7-AF9E-0C57D48DB5AC")), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);
                    break;
                }

                try
                {
                    using (ParametersContext _context = new())
                    {
                        ParametersStorageModelDB p = _context.SetStoredParameter(nameof(TelegramBotConfigModel), JsonConvert.SerializeObject(conf_tbot));
                    };
                    await PublishMessage(JsonConvert.SerializeObject(ResponseBaseModel.CreateSuccess("Данные успешно сохранены")), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);
                }
                catch (Exception ex)
                {
                    await PublishMessage(JsonConvert.SerializeObject(ResponseBaseModel.CreateError($"{ex.Message} // error {{9A97464F-6A50-4CE2-912E-0B0F6B6761BD}}")), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);
                }
                break;
            case $"{GlobalStatic.Routes.Storage}/{GlobalStatic.Routes.TelegramBot}/{GlobalStatic.Routes.GET}":
                req_noise = JsonConvert.DeserializeObject<NoiseModel>(payload_json);
                if (req_noise is null)
                {
                    _logger.LogError("req is null. C39195BC-C94E-442C-9724-3FABEF157648");
                    await PublishMessage(JsonConvert.SerializeObject(ResponseBaseModel.CreateError("req is null. C39195BC-C94E-442C-9724-3FABEF157648")), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);
                    break;
                }
                TelegramBotConfigResponseModel tbc_res = new();
                try
                {
                    using ParametersContext _context = new();
                    string _telegramBotConfig = _context.GetStoredParameter(nameof(TelegramBotConfigModel), "").StoredValue;
                    if (string.IsNullOrWhiteSpace(_telegramBotConfig))
                    {
                        tbc_res.AddError("Конфигурация не обнаружена error {BCE8ADDA-0A51-4B91-9751-A215015CF415}");
                        await PublishMessage(JsonConvert.SerializeObject(tbc_res), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);
                        break;
                    }
                    await PublishMessage(JsonConvert.SerializeObject(JsonConvert.DeserializeObject<TelegramBotConfigModel>(_telegramBotConfig) ?? new()), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);
                }
                catch (Exception ex)
                {
                    await PublishMessage(JsonConvert.SerializeObject(ResponseBaseModel.CreateError($"{ex} error {{763C9703-3D51-405B-AB09-B47DF584A0DE}}")), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);
                }

                break;
            case $"{GlobalStatic.Routes.Storage}/{GlobalStatic.Routes.TelegramBot}/{GlobalStatic.Routes.CHECK}":
                conf_tbot = JsonConvert.DeserializeObject<TelegramBotConfigModel>(payload_json);
                if (conf_tbot is null)
                {
                    _logger.LogError("req is null. 6814E514-AB2A-495E-93C7-B2E71787338F");
                    await PublishMessage(JsonConvert.SerializeObject(ResponseBaseModel.CreateError("req is null. 6814E514-AB2A-495E-93C7-B2E71787338F")), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);
                    break;
                }

                DictionaryResponseModel res = new();

                if (string.IsNullOrEmpty(conf_tbot?.TelegramBotToken))
                {
                    await PublishMessage(JsonConvert.SerializeObject(ResponseBaseModel.CreateError("TelegramBotToken не может быть пустым")), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);
                    break;
                }

                TelegramBotClientOptions options = new(conf_tbot.TelegramBotToken);

                if (!conf_tbot!.IsConfigured)
                {
                    await PublishMessage(JsonConvert.SerializeObject(ResponseBaseModel.CreateError("Конфигурация не установлена")), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);
                    break;
                }

                try
                {
                    TelegramBotClient tbot_cli = new(options, _http_client);
                    Telegram.Bot.Types.User _me = await tbot_cli.GetMeAsync();
                    res.DictionaryResponse = new Dictionary<string, object?>
                    {
                        { nameof(_me.FirstName), _me.FirstName },
                        { nameof(_me.Username), _me.Username },
                        { nameof(_me.Id), _me.Id },
                        { nameof(_me.IsBot), _me.IsBot }
                    };
                    await PublishMessage(JsonConvert.SerializeObject(res), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);
                }
                catch (Exception ex)
                {
                    await PublishMessage(JsonConvert.SerializeObject(ResponseBaseModel.CreateError(ex.Message)), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);
                }

                break;

            case $"{GlobalStatic.Routes.System}/{GlobalStatic.Routes.Commands}/{GlobalStatic.Routes.LIST}":
                req_noise = JsonConvert.DeserializeObject<NoiseModel>(payload_json);
                if (req_noise is null)
                {
                    _logger.LogError("req is null. error A10621EE-505C-4384-8D58-F0BBE7159492");
                    await PublishMessage(JsonConvert.SerializeObject(ResponseBaseModel.CreateError("req is null. error A10621EE-505C-4384-8D58-F0BBE7159492")), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);
                    break;
                }

                await PublishMessage(JsonConvert.SerializeObject(await _sys_com_service.CommandsGetAll()), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);
                break;
            case $"{GlobalStatic.Routes.System}/{GlobalStatic.Routes.Commands}/{GlobalStatic.Routes.UPDATE}":
                SystemCommandModelDB? req_sys_com_bd = JsonConvert.DeserializeObject<SystemCommandModelDB>(payload_json);
                if (req_sys_com_bd is null)
                {
                    _logger.LogError("req_sys_com_bd is null. error 23C49BF0-411D-4511-8006-5C4241D83EED");
                    await PublishMessage(JsonConvert.SerializeObject(ResponseBaseModel.CreateError("req_sys_com_bd is null. error 23C49BF0-411D-4511-8006-5C4241D83EED")), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);
                    break;
                }

                await PublishMessage(JsonConvert.SerializeObject(await _sys_com_service.CommandUpdateOrCreate(req_sys_com_bd)), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);
                break;
            case $"{GlobalStatic.Routes.System}/{GlobalStatic.Routes.Commands}/{GlobalStatic.Routes.START}":
                req_nid = JsonConvert.DeserializeObject<SimpleIdNoiseModel>(payload_json);
                if (req_nid is null)
                {
                    _logger.LogError("req_nid is null. error 7E1D1322-FA6D-4DDB-A9E3-1B61269445AE");
                    await PublishMessage(JsonConvert.SerializeObject(ResponseBaseModel.CreateError("req_nid is null. error 7E1D1322-FA6D-4DDB-A9E3-1B61269445AE")), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);
                    break;
                }

                await PublishMessage(JsonConvert.SerializeObject(await _sys_com_service.CommandRun(req_nid.Id)), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);
                break;
            case $"{GlobalStatic.Routes.System}/{GlobalStatic.Routes.Commands}/{GlobalStatic.Routes.DELETE}":
                req_nid = JsonConvert.DeserializeObject<SimpleIdNoiseModel>(payload_json);
                if (req_nid is null)
                {
                    _logger.LogError("req_nid is null. error 0E60251C-C3F0-47F7-8962-A7AECDF3D023");
                    await PublishMessage(JsonConvert.SerializeObject(ResponseBaseModel.CreateError("req_nid is null. error 0E60251C-C3F0-47F7-8962-A7AECDF3D023")), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);
                    break;
                }

                await PublishMessage(JsonConvert.SerializeObject(await _sys_com_service.CommandDelete(req_nid.Id)), e.ApplicationMessage.ResponseTopic, _mqtt_settings.Secret, salt);
                break;

            case $"{GlobalStatic.Routes.Users}/{GlobalStatic.Routes.GET}/{GlobalStatic.Routes.LIST}":

                break;
            case $"{GlobalStatic.Routes.Users}/{GlobalStatic.Routes.GET}":

                break;
            case $"{GlobalStatic.Routes.Users}/{GlobalStatic.Routes.UPDATE}":

                break;

            case GlobalStatic.Routes.SHOT:

                break;
            case GlobalStatic.Routes.Cameras:

                break;
            case GlobalStatic.Routes.AB_LOG_SYSTEM:

                break;
        }
    }

    /// <inheritdoc/>
    public async Task PublishMessage(string body_string, string topic, string? secret, string salt)
    {
        byte[] payload_bytes = await CipherService.EncryptAsync(body_string, secret ?? CipherService.DefaultSecret, salt);
        MqttPublishMessageModel pub_msg = new(payload_bytes, new[] { topic })
        {
            CorrelationData = Encoding.UTF8.GetBytes(salt),
        };

        if (!_mqttClient.IsConnected)
            await _mqttClient.ConnectAsync(MqttClientOptions);

        MqttPublishMessageResultModel pub_res = await PublishMessage(pub_msg);

        if (pub_res?.IsSuccess != true)
        {
            _logger.LogError($"!pub_red.IsSuccess ({pub_res?.Message}). error FAB191D8-4648-4705-AEFF-DF0AB762E527");
        }
    }
}