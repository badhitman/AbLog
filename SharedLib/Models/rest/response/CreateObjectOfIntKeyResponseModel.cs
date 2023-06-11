using Newtonsoft.Json;

namespace SharedLib
{
    /// <summary>
    /// Результат запроса создания объекта (ключ создаваемого объекта: int)
    /// </summary>
    public class CreateObjectOfIntKeyResponseModel : ResponseBaseModel
    {
        /// <summary>
        /// Результат запроса создания объекта (ключ создаваемого объекта: int)
        /// </summary>
        public CreateObjectOfIntKeyResponseModel() { }

        /// <summary>
        /// Результат запроса создания объекта (ключ создаваемого объекта: int)
        /// </summary>
        /// <param name="obj">Исходный объект для воссоздания</param>
        public CreateObjectOfIntKeyResponseModel(ResponseBaseModel obj)
        {
            foreach (ResultMessage msg in obj.Messages)
                AddMessage(msg.TypeMessage, msg.Text);
        }

        /// <summary>
        /// Идентификатор нового созданного объекта
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}