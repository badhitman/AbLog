namespace SharedLib
{
    /// <summary>
    /// Конфигурация подключения к инфраструктуре обмена сообщениями
    /// </summary>
    public interface IParametersStorageService
    {
        /// <summary>
        /// Сохранить конфигурацию Email подключения
        /// </summary>
        public Task<ResponseBaseModel> SaveEmailConfig(EmailConfigModel connect_config);

        /// <summary>
        /// Получить конфигурацию Email подключения
        /// </summary>
        public Task<EmailConfigModel> GetEmailConfig();

        /// <summary>
        /// Тест Email подключения (SMTP)
        /// </summary>
        public Task<ResponseBaseModel> TestEmailConnect(EmailConfigModel? conf = null);
    }
}
