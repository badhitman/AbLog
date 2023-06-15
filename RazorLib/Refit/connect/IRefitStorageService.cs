using SharedLib;
using Refit;

namespace BlazorLib
{
    /// <summary>
    /// Работа с бэком: Storage
    /// </summary>
    [Headers("Content-Type: application/json")]
    public interface IRefitStorageService
    {
        /// <summary>
        /// Получить конфигурацию Email (imap+smtp)
        /// </summary>
        [Get($"/api/{GlobalStatic.HttpRoutes.Storage}/{GlobalStatic.HttpRoutes.GET_EMAIL_CONFIG}")]
        public Task<ApiResponse<EmailConfigModel>> EmailConfigGet();

        /// <summary>
        /// Сохранить конфигурацию Email (imap+smtp)
        /// </summary>
        [Post($"/api/{GlobalStatic.HttpRoutes.Storage}/{GlobalStatic.HttpRoutes.SAVE_EMAIL_CONFIG}")]
        public Task<ApiResponse<ResponseBaseModel>> EmailConfigSave(EmailConfigModel email_conf);

        /// <summary>
        /// Проверить подключение к Email (конфигурация imap+smtp)
        /// </summary>
        [Post($"/api/{GlobalStatic.HttpRoutes.Storage}/{GlobalStatic.HttpRoutes.TEST_CONNECTION_EMAIL_CONFIG}")]
        public Task<ApiResponse<ResponseBaseModel>> EmailConfigTestSmtpConnection(EmailConfigModel? email_conf);
    }
}