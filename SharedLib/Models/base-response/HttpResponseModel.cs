////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using System.Net;

namespace SharedLib;

/// <summary>
/// Ответ/результат на HTTP запрос
/// </summary>
public class HttpResponseModel : SimpleStringResponseModel
{
    /// <inheritdoc/>
    public static HttpResponseModel Build(ResponseBaseModel responseBaseModel)
    {
        return new HttpResponseModel()
        {
            Messages = responseBaseModel.Messages
        };
    }

    /// <inheritdoc/>
    public override bool IsSuccess => base.IsSuccess && (int)StatusCode >= 200 && (int)StatusCode < 300;

    /// <summary>
    /// HTTP Sattus code
    /// </summary>
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

    /// <summary>
    /// 
    /// </summary>
    public async Task<HtmlDomModel> GetDom()
    {
        HtmlDomModel res = [];
        await res.Reload(TextPayload ?? "");
        return res;
    }
}