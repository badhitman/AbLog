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
        readonly IRefitService _refit_hardwares;

        /// <summary>
        /// Устройства
        /// </summary>
        public HardwaresRefitService(IRefitService refit_hardwares)
        {
            _refit_hardwares = refit_hardwares;
        }

        /// <inheritdoc/>
        public async Task<HardwareResponseModel> HardwareGet(int hardware_id)
        {
            Refit.ApiResponse<HardwareResponseModel> rest = await _refit_hardwares.HardwareGet(hardware_id);

            if (rest.Content is null)
                return (HardwareResponseModel)ResponseBaseModel.CreateError("rest.Content is null // error {8350F763-2882-48EF-9DD1-005232416473}");

            return rest.Content;
        }

        /// <inheritdoc/>
        public async Task<PortHardwareResponseModel> HardwarePortGet(int port_id)
        {
            Refit.ApiResponse<PortHardwareResponseModel> rest = await _refit_hardwares.HardwarePortGet(port_id);

            if (rest.Content is null)
                return (PortHardwareResponseModel)ResponseBaseModel.CreateError("rest.Content is null // error {D4E70D4C-85C6-4A89-A596-DB05D33AA6DC}");

            return rest.Content;
        }

        /// <inheritdoc/>
        public async Task<HardwaresResponseModel> HardwaresGetAll()
        {
            Refit.ApiResponse<HardwaresResponseModel> rest = await _refit_hardwares.HardwaresGetAll();

            if (rest.Content is null)
                return (HardwaresResponseModel)ResponseBaseModel.CreateError("rest.Content is null // error {5018BEE0-6931-4A0F-A562-C07B1FE78E25}");

            return rest.Content;
        }

        /// <inheritdoc/>
        public async Task<EntriesResponseModel> HardwaresGetAllAsEntries()
        {
            Refit.ApiResponse<EntriesResponseModel> rest = await _refit_hardwares.HardwaresGetAllAsEntries();

            if (rest.Content is null)
                return (EntriesResponseModel)ResponseBaseModel.CreateError("rest.Content is null // error {175FC2D3-699B-4514-A422-6662A08F6C0D}");

            return rest.Content;
        }

        /// <inheritdoc/>
        public async Task<EntriesNestedResponseModel> HardwaresGetTreeNestedEntries()
        {
            Refit.ApiResponse<EntriesNestedResponseModel> rest = await _refit_hardwares.HardwaresGetTreeNestedEntries();

            if (rest.Content is null)
                return (EntriesNestedResponseModel)ResponseBaseModel.CreateError("rest.Content is null // error {A247646C-FF9A-4572-B356-3579A4AE98CD}");

            return rest.Content;
        }
    }
}
