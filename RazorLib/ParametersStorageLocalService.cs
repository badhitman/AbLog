using SharedLib;

namespace BlazorLib
{
    /// <summary>
    /// Конфигурация подключения Email (Refit)
    /// </summary>
    public class ParametersStorageLocalService : IParametersStorageService
    {
        /// <inheritdoc/>
        public Task<EmailConfigModel> GetEmailConfig()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<ResponseBaseModel> SaveEmailConfig(EmailConfigModel connect_config)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<ResponseBaseModel> TestEmailConnect(EmailConfigModel? conf = null)
        {
            throw new NotImplementedException();
        }
    }
}