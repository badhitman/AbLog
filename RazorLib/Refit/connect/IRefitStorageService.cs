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
        #region Email

        /// <summary>
        /// Получить конфигурацию Email (imap+smtp)
        /// </summary>
        [Get($"/api/{GlobalStatic.HttpRoutes.Storage}/{GlobalStatic.HttpRoutes.Email}/{GlobalStatic.HttpRoutes.GET}")]
        public Task<ApiResponse<EmailConfigResponseModel>> EmailConfigGet();

        /// <summary>
        /// Сохранить конфигурацию Email (imap+smtp)
        /// </summary>
        [Post($"/api/{GlobalStatic.HttpRoutes.Storage}/{GlobalStatic.HttpRoutes.Email}/{GlobalStatic.HttpRoutes.UPDATE}")]
        public Task<ApiResponse<ResponseBaseModel>> EmailConfigSave(EmailConfigModel email_conf);

        /// <summary>
        /// Проверить подключение к Email (конфигурация imap+smtp)
        /// </summary>
        [Post($"/api/{GlobalStatic.HttpRoutes.Storage}/{GlobalStatic.HttpRoutes.Email}/{GlobalStatic.HttpRoutes.CHECK}")]
        public Task<ApiResponse<ResponseBaseModel>> EmailConfigTestSmtpConnection(EmailConfigModel? email_conf);

        #endregion

        #region Mqtt

        /// <summary>
        /// Получить конфигурацию Mqtt
        /// </summary>
        [Get($"/api/{GlobalStatic.HttpRoutes.Storage}/{GlobalStatic.HttpRoutes.Mqtt}/{GlobalStatic.HttpRoutes.GET}")]
        public Task<ApiResponse<MqttConfigResponseModel>> MqttConfigGet();

        /// <summary>
        /// Сохранить конфигурацию Mqtt
        /// </summary>
        [Post($"/api/{GlobalStatic.HttpRoutes.Storage}/{GlobalStatic.HttpRoutes.Mqtt}/{GlobalStatic.HttpRoutes.UPDATE}")]
        public Task<ApiResponse<ResponseBaseModel>> MqttConfigSave(MqttConfigModel mqtt_conf);

        /// <summary>
        /// Проверить подключение к Mqtt
        /// </summary>
        [Post($"/api/{GlobalStatic.HttpRoutes.Storage}/{GlobalStatic.HttpRoutes.Mqtt}/{GlobalStatic.HttpRoutes.CHECK}")]
        public Task<ApiResponse<ResponseBaseModel>> MqttConfigTestSmtpConnection(MqttConfigModel? mqtt_conf);

        #endregion
    }
}