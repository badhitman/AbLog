using System.Net;

namespace SharedLib
{
    /// <summary>
    /// Ответ/результат на HTTP запрос
    /// </summary>
    public class HttpResponseModel : TextedResponseModel
    {
        /// <summary>
        /// Ответ/результат на HTTP запрос
        /// </summary>
        public HttpResponseModel(ResponseBaseModel responseBaseModel)
        {
            Messages = responseBaseModel.Messages;
        }

        /// <summary>
        /// Ответ/результат на HTTP запрос
        /// </summary>
        public HttpResponseModel()
        {
        }

        /// <inheritdoc/>
        public override bool IsSuccess => base.IsSuccess && (int)StatusCode >= 200 && (int)StatusCode < 300;

        /// <summary>
        /// HTTP Sattus code
        /// </summary>
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
    }
}