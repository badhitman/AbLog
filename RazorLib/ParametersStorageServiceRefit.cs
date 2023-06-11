using SharedLib;

namespace BlazorLib
{
    /// <summary>
    /// Конфигурация подключения Email (Refit)
    /// </summary>
    public class ParametersStorageServiceRefit : IParametersStorageService
    {
        readonly IRefitService _refit;

        /// <summary>
        /// Конфигурация подключения Email (Refit)
        /// </summary>
        public ParametersStorageServiceRefit(IRefitService set_refit)
        {
            _refit = set_refit;
        }

        /// <inheritdoc/>
        public async Task<EmailConfigModel> GetEmailConfig()
        {
            Refit.ApiResponse<EmailConfigModel> rest = await _refit.GetEmailConfig();
            if (rest.Content is null)
                throw new Exception($"{nameof(_refit.GetEmailConfig)}.Content is null {{FB992C9A-AF31-425F-AFF6-11077C08F6C7}}");

            return rest.Content;
        }

        /// <inheritdoc/>
        public async Task<ResponseBaseModel> SaveEmailConfig(EmailConfigModel connect_config)
        {
            Refit.ApiResponse<ResponseBaseModel> rest = await _refit.SaveEmailConfig(connect_config);
            if (rest.Content is null)
                throw new Exception($"{nameof(_refit.SaveEmailConfig)}.Content is null {{ACC718CF-C7A2-4F8B-BE20-05490701BF1D}}");

            return rest.Content;
        }

        /// <inheritdoc/>
        public async Task<ResponseBaseModel> TestEmailConnect(EmailConfigModel? conf = null)
        {
            Refit.ApiResponse<ResponseBaseModel> rest = await _refit.TestEmailConfigSmtpConnection(conf);
            if (rest.Content is null)
                throw new Exception($"{nameof(_refit.TestEmailConfigSmtpConnection)}.Content is null {{BC0D377C-03BF-42FB-B5DF-3667B98FB7F2}}");

            return rest.Content;
        }
    }
}