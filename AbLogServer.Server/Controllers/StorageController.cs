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
        private readonly ILogger<StorageController> _logger;
        private readonly IParametersStorageService _parameters_storage;

        /// <summary>
        /// 
        /// </summary>
        public StorageController(ILogger<StorageController> logger, IParametersStorageService parameters_storage)
        {
            _logger = logger;
            _parameters_storage = parameters_storage;
        }

        /// <summary>
        /// Получить конфигурацию Email (imap+smtp)
        /// </summary>
        [HttpGet($"{GlobalStatic.HttpRoutes.GET_EMAIL_CONFIG}")]
        public async Task<EmailConfigModel> EmailConfigGet()
        {
            return await _parameters_storage.GetEmailConfig();
        }

        /// <summary>
        /// Сохранить конфигурацию Email (imap+smtp)
        /// </summary>
        [HttpPost($"{GlobalStatic.HttpRoutes.SAVE_EMAIL_CONFIG}")]
        public async Task<ResponseBaseModel> EmailConfigSave(EmailConfigModel e_conf)
        {
            ResponseBaseModel res_b = new();

            if (e_conf is null)
            {
                res_b.AddError("Ошибка выполнения запроса: {303B97D1-671F-425D-BF19-C6E480440FEA}");
                return res_b;
            }
            ResponseBaseModel pr = await _parameters_storage.SaveEmailConfig(e_conf);
            res_b.AddMessages(pr.Messages);

            return res_b;
        }

        /// <summary>
        /// Проверить подключение к Email (конфигурация imap+smtp)
        /// </summary>
        [HttpPost($"{GlobalStatic.HttpRoutes.TEST_CONNECTION_EMAIL_CONFIG}")]
        public async Task<ResponseBaseModel> EmailConfigTestSmtpConnection(EmailConfigModel? email_conf)
        {
            return await _parameters_storage.TestEmailConnect(email_conf);
        }
    }
}