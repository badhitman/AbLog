using Newtonsoft.Json;
using SharedLib;
using MimeKit;
using MailKit.Net.Smtp;
using System.Net.Mail;
using ab.context;

namespace ServicesLib
{
    /// <summary>
    /// Конфигурация подключения Email
    /// </summary>
    public class ParametersStorageService : IParametersStorageService
    {
        /// <inheritdoc/>
        public Task<ResponseBaseModel> SaveEmailConfig(EmailConfigModel connect_config)
        {
            using StorageParametersContext _context = new();
            ParametersStorageModelDB p = _context.SetStoredParameter(nameof(EmailConfigModel), JsonConvert.SerializeObject(connect_config));
            ResponseBaseModel res = new();
            res.AddSuccess($"Данные успешно записаны в БД #{p.Id}");
            return Task.FromResult(res);
        }

        /// <inheritdoc/>
        public Task<EmailConfigModel> GetEmailConfig()
        {
            using StorageParametersContext _context = new();
            string _emailConfig = _context.GetStoredParameter(nameof(EmailConfigModel), "").StoredValue;
            if (string.IsNullOrWhiteSpace(_emailConfig))
                return Task.FromResult(new EmailConfigModel());

            EmailConfigModel res = JsonConvert.DeserializeObject<EmailConfigModel>(_emailConfig) ?? new();
            /*
             [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("linux")]
        [SupportedOSPlatform("android")]
             */
            return Task.FromResult(res);
        }

        /// <inheritdoc/>
        public async Task<ResponseBaseModel> TestEmailConnect(EmailConfigModel? conf = null)
        {
            ResponseBaseModel res = new();
            conf ??= await GetEmailConfig();
            if (!conf.IsConfigured)
            {
                res.AddError("Конфигурация не установлена");
                return res;
            }

            using EmailService emailService = new();
            res = await emailService.ConnectSmtpAsync(conf);
            ResponseBaseModel res2 = await emailService.ConnectImapAsync(conf);
            res.AddMessages(res2.Messages);
            return res;
        }
    }
}