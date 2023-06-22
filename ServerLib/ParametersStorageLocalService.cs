using Newtonsoft.Json;
using MQTTnet.Client;
using ab.context;
using SharedLib;
using MQTTnet;

namespace ServicesLib
{
    /// <summary>
    /// Хранение параметров IMqttClient
    /// </summary>
    public class ParametersStorageLocalService : IParametersStorageService
    {
        readonly IMqttClient _mqtt;

        /// <summary>
        /// Хранение параметров IMqttClient
        /// </summary>
        public ParametersStorageLocalService(MqttFactory mqtt)
        {
            _mqtt = mqtt.CreateMqttClient(); ;
        }

        #region Email

        /// <inheritdoc/>
        public Task<ResponseBaseModel> SaveEmailConfig(EmailConfigModel connect_config)
        {
            using ServerContext _context = new();
            ParametersStorageModelDB p = _context.SetStoredParameter(nameof(EmailConfigModel), JsonConvert.SerializeObject(connect_config));
            ResponseBaseModel res = new();
            res.AddSuccess($"Данные успешно записаны в БД #{p.Id}");
            return Task.FromResult(res);
        }

        /// <inheritdoc/>
        public Task<EmailConfigResponseModel> GetEmailConfig()
        {
            EmailConfigResponseModel res = new();
            using ServerContext _context = new();
            string _emailConfig = _context.GetStoredParameter(nameof(EmailConfigModel), "").StoredValue;
            if (string.IsNullOrWhiteSpace(_emailConfig))
                return Task.FromResult(res);

            res.Conf = JsonConvert.DeserializeObject<EmailConfigModel>(_emailConfig) ?? new();

            return Task.FromResult(res);
        }

        /// <inheritdoc/>
        public async Task<ResponseBaseModel> TestEmailConnect(EmailConfigModel? conf = null)
        {
            ResponseBaseModel res = new();
            conf ??= (await GetEmailConfig()).Conf;
            if (!conf.IsConfigured)
            {
                res.AddError("Конфигурация не установлена");
                return res;
            }

            using EmailLocalService emailService = new();
            res = await emailService.ConnectSmtpAsync(conf);
            ResponseBaseModel res2 = await emailService.ConnectImapAsync(conf);
            res.AddMessages(res2.Messages);
            return res;
        }

        #endregion

        #region Mqtt

        /// <inheritdoc/>
        public Task<ResponseBaseModel> SaveMqttConfig(MqttConfigModel connect_config)
        {
            using ServerContext _context = new();
            ParametersStorageModelDB p = _context.SetStoredParameter(nameof(MqttConfigModel), JsonConvert.SerializeObject(connect_config));
            ResponseBaseModel res = new();
            res.AddSuccess($"Данные успешно записаны в БД #{p.Id}");
            return Task.FromResult(res);
        }

        /// <inheritdoc/>
        public Task<MqttConfigResponseModel> GetMqttConfig()
        {
            MqttConfigResponseModel res = new();
            using ServerContext _context = new();
            string _mqttConfig = _context.GetStoredParameter(nameof(MqttConfigModel), "").StoredValue;
            if (string.IsNullOrWhiteSpace(_mqttConfig))
                return Task.FromResult(res);

            res.Conf = JsonConvert.DeserializeObject<MqttConfigModel>(_mqttConfig) ?? new();

            return Task.FromResult(res);
        }

        /// <inheritdoc/>
        public async Task<ResponseBaseModel> TestMqttConnect(MqttConfigModel? conf = null)
        {
            ResponseBaseModel res = new();
            conf ??= (await GetMqttConfig()).Conf;
            if (!conf!.IsConfigured)
            {
                res.AddError("Конфигурация не установлена");
                return res;
            }

            MqttClientOptions mqttClientOptions = new MqttClientOptionsBuilder()
               .WithTls()
               .WithClientId(conf.ClientId)
               .WithTcpServer(conf.Server, conf.Port)
               .WithCredentials(conf.Username, conf.Password)
               .WithCleanSession()
            .Build();

            MqttClientConnectResult connect_res = await _mqtt.ConnectAsync(mqttClientOptions, CancellationToken.None);

            if (connect_res.ResultCode != MqttClientConnectResultCode.Success)
                res.AddError(connect_res.ResultCode.ToString());
            else
            {
                res.AddSuccess("Подключение успешно");
                await _mqtt.TryDisconnectAsync();
                _mqtt.Dispose();
            }

            return res;
        }

        #endregion
    }
}