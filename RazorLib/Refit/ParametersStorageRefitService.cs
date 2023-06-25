using SharedLib;

namespace BlazorLib
{
    /// <summary>
    /// Хранение параметров в БД
    /// </summary>
    public class ParametersStorageRefitService : IParametersStorageService
    {
        readonly IRefitStorageService _refit;

        /// <summary>
        /// Хранение параметров в БД
        /// </summary>
        public ParametersStorageRefitService(IRefitStorageService set_refit)
        {
            _refit = set_refit;
        }

        #region Email

        /// <inheritdoc/>
        public async Task<EmailConfigResponseModel> GetEmailConfig()
        {
            EmailConfigResponseModel res = new();
            Refit.ApiResponse<EmailConfigResponseModel> rest = await _refit.EmailConfigGet();
            if (rest.Content is null || !rest.IsSuccessStatusCode)
                res.AddError($"rest.Content is null || !rest.IsSuccessStatusCode {{FB992C9A-AF31-425F-AFF6-11077C08F6C7}}");

            if (rest.Error is not null)
                res.AddError(rest.Error.Message);

            if (!res.IsSuccess)
                return res;

            return rest.Content!;
        }

        /// <inheritdoc/>
        public async Task<ResponseBaseModel> SaveEmailConfig(EmailConfigModel connect_config)
        {
            ResponseBaseModel res = new();
            Refit.ApiResponse<ResponseBaseModel> rest = await _refit.EmailConfigSave(connect_config);
            if (rest.Content is null || !rest.IsSuccessStatusCode)
                res.AddError($"rest.Content is null || !rest.IsSuccessStatusCode {{ACC718CF-C7A2-4F8B-BE20-05490701BF1D}}");

            if (rest.Error is not null)
                res.AddError(rest.Error.Message);

            if (!res.IsSuccess)
                return res;

            return rest.Content!;
        }

        #endregion

        #region Mqtt

        /// <inheritdoc/>
        public async Task<MqttConfigResponseModel> GetMqttConfig()
        {
            MqttConfigResponseModel res = new();
            Refit.ApiResponse<MqttConfigResponseModel> rest = await _refit.MqttConfigGet();
            if (rest.Content is null || !rest.IsSuccessStatusCode)
                res.AddError($"rest.Content is null || !rest.IsSuccessStatusCode {{CD3992C4-9C07-4569-9E61-63956DBE74AB}}");

            if (rest.Error is not null)
                res.AddError(rest.Error.Message);

            if (!res.IsSuccess)
                return res;

            return rest.Content!;
        }

        /// <inheritdoc/>
        public async Task<ResponseBaseModel> SaveMqttConfig(MqttConfigModel connect_config)
        {
            ResponseBaseModel res = new();
            Refit.ApiResponse<ResponseBaseModel> rest = await _refit.MqttConfigSave(connect_config);
            if (rest.Content is null || !rest.IsSuccessStatusCode)
                res.AddError($"rest.Content is null || !rest.IsSuccessStatusCode {{8343079A-1D01-41FB-8B13-66A3E66B8AF5}}");
            if (rest.Error is not null)
                res.AddError(rest.Error.Message);

            if (!res.IsSuccess)
                return res;

            return rest.Content!;
        }

        #endregion
    }
}