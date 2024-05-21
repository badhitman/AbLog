////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using System.Net;

namespace SharedLib;

/// <summary>
/// Ответ/результат на HTTP запрос
/// </summary>
public class HttpResponseModel : TResponseModel<string>
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
    /// HTTP Status code
    /// </summary>
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

    /// <summary>
    /// 
    /// </summary>
    public async Task<HtmlDomModel> GetDom()
    {
        HtmlDomModel res = [];
        await res.Reload(Response ?? "");
        return res;
    }
}