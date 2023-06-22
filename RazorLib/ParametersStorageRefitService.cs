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
            if (rest.Content is null)
            {
                res.AddError($"{nameof(_refit.EmailConfigGet)}.Content is null {{FB992C9A-AF31-425F-AFF6-11077C08F6C7}}");
                return res;
            }

            if (rest.Error is not null)
            {
                res.AddError(rest.Error.Message);
                return res;
            }

            res.Conf = rest.Content.Conf;
            return res;
        }

        /// <inheritdoc/>
        public async Task<ResponseBaseModel> SaveEmailConfig(EmailConfigModel connect_config)
        {
            Refit.ApiResponse<ResponseBaseModel> rest = await _refit.EmailConfigSave(connect_config);
            if (rest.Content is null)
                throw new Exception($"{nameof(_refit.EmailConfigSave)}.Content is null {{ACC718CF-C7A2-4F8B-BE20-05490701BF1D}}");

            return rest.Content;
        }

        /// <inheritdoc/>
        public async Task<ResponseBaseModel> TestEmailConnect(EmailConfigModel? conf = null)
        {
            Refit.ApiResponse<ResponseBaseModel> rest = await _refit.EmailConfigTestSmtpConnection(conf);
            if (rest.Content is null)
                throw new Exception($"{nameof(_refit.EmailConfigTestSmtpConnection)}.Content is null {{BC0D377C-03BF-42FB-B5DF-3667B98FB7F2}}");

            return rest.Content;
        }

        #endregion

        #region Mqtt

        /// <inheritdoc/>
        public async Task<MqttConfigResponseModel> GetMqttConfig()
        {
            MqttConfigResponseModel res = new();
            Refit.ApiResponse<MqttConfigResponseModel> rest = await _refit.MqttConfigGet();
            if (rest.Content is null)
            {
                res.AddError($"{nameof(_refit.MqttConfigGet)}.Content is null {{CD3992C4-9C07-4569-9E61-63956DBE74AB}}");
                return res;
            }

            if (rest.Error is not null)
            {
                res.AddError(rest.Error.Message);
                return res;
            }

            res.Conf = rest.Content.Conf;

            return res;
        }

        /// <inheritdoc/>
        public async Task<ResponseBaseModel> SaveMqttConfig(MqttConfigModel connect_config)
        {
            Refit.ApiResponse<ResponseBaseModel> rest = await _refit.MqttConfigSave(connect_config);
            if (rest.Content is null)
                throw new Exception($"{nameof(_refit.MqttConfigSave)}.Content is null {{8343079A-1D01-41FB-8B13-66A3E66B8AF5}}");

            return rest.Content;
        }

        /// <inheritdoc/>
        public async Task<ResponseBaseModel> TestMqttConnect(MqttConfigModel? conf = null)
        {
            Refit.ApiResponse<ResponseBaseModel> rest = await _refit.MqttConfigTestSmtpConnection(conf);
            if (rest.Content is null)
                throw new Exception($"{nameof(_refit.MqttConfigTestSmtpConnection)}.Content is null {{3657D4F9-E721-4172-AC9E-82A49D04198D}}");

            return rest.Content;
        }

        #endregion
    }
}