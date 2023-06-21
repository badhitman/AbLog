using SharedLib.IServices;
using BlazorLib;
using SharedLib;

namespace RazorLib
{
    /// <summary>
    /// Устройства
    /// </summary>
    public class HardwaresRefitService : IHardwaresService
    {
        readonly IRefitHardwaresService _refit_hardwares;

        /// <summary>
        /// Устройства
        /// </summary>
        public HardwaresRefitService(IRefitHardwaresService refit_hardwares)
        {
            _refit_hardwares = refit_hardwares;
        }

        /// <inheritdoc/>
        public async Task<HttpResponseModel> GetHardwareHtmlPage(HardvareGetRequestModel req)
        {
            Refit.ApiResponse<HttpResponseModel> rest = await _refit_hardwares.GetHardwareHtmlPage(req);

            if (rest.Content is null)
                return new HttpResponseModel(ResponseBaseModel.CreateError("rest.Content is null // error {26ABE035-F428-4433-9BA6-9D8937051942}"));

            return rest.Content;
        }

        /// <inheritdoc/>
        public async Task<HardwareResponseModel> HardwareGet(int hardware_id)
        {
            HardwareResponseModel res = new();
            Refit.ApiResponse<HardwareResponseModel> rest = await _refit_hardwares.HardwareGet(hardware_id);

            if (rest.Content is null)
            {
                res.AddError("rest.Content is null // error {0CA3B140-267D-47C9-B094-62E3D07523B2}");
                if (rest.Error is not null)
                    res.AddError(rest.Error.Message);
            }
            else
                res = rest.Content;

            return res;
        }

        /// <inheritdoc/>
        public async Task<PortHardwareResponseModel> HardwarePortGet(int port_id)
        {
            PortHardwareResponseModel res = new();
            Refit.ApiResponse<PortHardwareResponseModel> rest = await _refit_hardwares.HardwarePortGet(port_id);

            if (rest.Content is null)
            {
                res.AddError("rest.Content is null // error {2A5377F9-CFBB-432D-941C-F2DD50D43238}");
                if (rest.Error is not null)
                    res.AddError(rest.Error.Message);
            }
            else
                res = rest.Content;

            return res;
        }

        /// <inheritdoc/>
        public async Task<HardwaresResponseModel> HardwaresGetAll()
        {
            HardwaresResponseModel res = new();
            Refit.ApiResponse<HardwaresResponseModel> rest = await _refit_hardwares.HardwaresGetAll();

            if (rest.Content is null)
            {
                res.AddError("rest.Content is null // error {B51CD9F7-B48A-4949-9F3E-FB561EC08959}");
                if (rest.Error is not null)
                    res.AddError(rest.Error.Message);
            }
            else
                res = rest.Content;

            return res;
        }

        /// <inheritdoc/>
        public async Task<EntriesResponseModel> HardwaresGetAllAsEntries()
        {
            EntriesResponseModel res = new();
            Refit.ApiResponse<EntriesResponseModel> rest = await _refit_hardwares.HardwaresGetAllAsEntries();

            if (rest.Content is null)
            {
                res.AddError("rest.Content is null // error {C5D2189B-2E1E-4C83-BE8E-8AF5600B7DB4}");
                if (rest.Error is not null)
                    res.AddError(rest.Error.Message);
            }
            else
                res = rest.Content;

            return res;
        }

        /// <inheritdoc/>
        public async Task<EntriesNestedResponseModel> HardwaresGetTreeNestedEntries()
        {
            EntriesNestedResponseModel res = new();
            Refit.ApiResponse<EntriesNestedResponseModel> rest = await _refit_hardwares.HardwaresGetTreeNestedEntries();

            if (rest.Content is null)
            {
                res.AddError("rest.Content is null // error {0E2597BA-C8AA-4B6F-843B-08ED1B6F1C9E}");
                if (rest.Error is not null)
                    res.AddError(rest.Error.Message);
            }
            else
                res = rest.Content;

            return res;
        }

        /// <inheritdoc/>
        public async Task<EntriyResponseModel> CheckPortHardware(PortHardwareCheckRequestModel req)
        {
            EntriyResponseModel res = new();
            Refit.ApiResponse<EntriyResponseModel> rest = await _refit_hardwares.CheckPortHardware(req);

            if (rest.Content is null)
            {
                res.AddError("rest.Content is null // error {22228B6C-C216-45BE-A861-BE41ECC0EDE3}");
                if (rest.Error is not null)
                    res.AddError(rest.Error.Message);
            }
            else
                res = rest.Content;

            return res;
        }

        /// <inheritdoc/>
        public async Task<HardwareResponseModel> HardwareUpdate(HardwareBaseModel hardware)
        {
            HardwareResponseModel res = new();

            Refit.ApiResponse<HardwareResponseModel> rest = await _refit_hardwares.HardwareUpdate(hardware);

            if (rest.Content is null)
            {
                res.AddError("rest.Content is null // error {8C403734-E575-46E4-A784-17742D0AEF97}");
                if (rest.Error is not null)
                    res.AddError(rest.Error.Message);
            }
            else
                res = rest.Content;

            return res;
        }

        /// <inheritdoc/>
        public async Task<ResponseBaseModel> SetNamePort(EntryModel port_id_name)
        {
            Refit.ApiResponse<ResponseBaseModel> rest = await _refit_hardwares.SetNamePort(port_id_name);

            if (rest.Content is null)
                return ResponseBaseModel.CreateError("rest.Content is null // error {1A93C004-2AC6-4541-8351-D15B349CC7B1}");

            return rest.Content;
        }

        /// <inheritdoc/>
        public async Task<ResponseBaseModel> HardwareDelete(int hardware_id)
        {
            Refit.ApiResponse<ResponseBaseModel> rest = await _refit_hardwares.HardwareDelete(hardware_id);

            if (rest.Content is null)
                return ResponseBaseModel.CreateError("rest.Content is null // error {E5DDF9CE-8C59-4E5B-802D-027BCE753BCA}");

            return rest.Content;
        }
    }
}
