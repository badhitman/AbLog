using Microsoft.AspNetCore.Mvc;
using SharedLib;

namespace AbLogServer.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("/api/[controller]")]
    public class StorageController : ControllerBase
    {
        private readonly IParametersStorageService _parameters_storage;
        private readonly ILogger<StorageController> _logger;

        /// <summary>
        /// 
        /// </summary>
        public StorageController(ILogger<StorageController> logger, IParametersStorageService parameters_storage)
        {
            _logger = logger;
            _parameters_storage = parameters_storage;
        }

        #region Email

        /// <summary>
        /// Получить конфигурацию Email (imap+smtp)
        /// </summary>
        [HttpGet($"{GlobalStatic.HttpRoutes.Email}/{GlobalStatic.HttpRoutes.GET}")]
        public async Task<EmailConfigResponseModel> EmailConfigGet() => await _parameters_storage.GetEmailConfig();

        /// <summary>
        /// Сохранить конфигурацию Email (imap+smtp)
        /// </summary>
        [HttpPost($"{GlobalStatic.HttpRoutes.Email}/{GlobalStatic.HttpRoutes.UPDATE}")]
        public async Task<ResponseBaseModel> EmailConfigSave(EmailConfigModel e_conf) => await _parameters_storage.SaveEmailConfig(e_conf);

        /// <summary>
        /// Проверить подключение к Email (конфигурация imap+smtp)
        /// </summary>
        [HttpPost($"{GlobalStatic.HttpRoutes.Email}/{GlobalStatic.HttpRoutes.CHECK}")]
        public async Task<ResponseBaseModel> EmailConfigTestSmtpConnection(EmailConfigModel? email_conf) => await _parameters_storage.TestEmailConnect(email_conf);

        #endregion

        #region MQTT

        /// <summary>
        /// Получить конфигурацию Mqtt
        /// </summary>
        [HttpGet($"{GlobalStatic.HttpRoutes.Mqtt}/{GlobalStatic.HttpRoutes.GET}")]
        public async Task<MqttConfigResponseModel> MqttConfigGet() => await _parameters_storage.GetMqttConfig();

        /// <summary>
        /// Сохранить конфигурацию Mqtt (imap+smtp)
        /// </summary>
        [HttpPost($"{GlobalStatic.HttpRoutes.Mqtt}/{GlobalStatic.HttpRoutes.UPDATE}")]
        public async Task<ResponseBaseModel> MqttConfigSave(MqttConfigModel e_conf) => await _parameters_storage.SaveMqttConfig(e_conf);

        /// <summary>
        /// Проверить подключение к Mqtt
        /// </summary>
        [HttpPost($"{GlobalStatic.HttpRoutes.Mqtt}/{GlobalStatic.HttpRoutes.CHECK}")]
        public async Task<ResponseBaseModel> MqttConfigTestConnection(MqttConfigModel? mqtt_conf) => await _parameters_storage.TestMqttConnect(mqtt_conf);

        #endregion
    }
}