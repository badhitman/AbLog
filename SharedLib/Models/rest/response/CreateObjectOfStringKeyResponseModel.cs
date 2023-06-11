using Newtonsoft.Json;

namespace SharedLib
{
    /// <summary>
    /// Результат запроса создания объекта (ключ создаваемого объекта: string)
    /// </summary>
    public class CreateObjectOfStringKeyResponseModel : ResponseBaseModel
    {
        /// <summary>
        /// Результат запроса создания объекта (ключ создаваемого объекта: string)
        /// </summary>
        public CreateObjectOfStringKeyResponseModel() { }

        /// <summary>
        /// Результат запроса создания объекта (ключ создаваемого объекта: string)
        /// </summary>
        /// <param name="obj">Исходный объект для воссоздания</param>
        public CreateObjectOfStringKeyResponseModel(ResponseBaseModel obj)
        {
            foreach (ResultMessage msg in obj.Messages)
                AddMessage(msg.TypeMessage, msg.Text);
        }

        /// <summary>
        /// Идентификатор нового созданного объекта
        /// </summary>
        [JsonProperty("id")]
        public string? Id { get; set; }
    }
}