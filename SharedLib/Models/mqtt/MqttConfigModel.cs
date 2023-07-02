using System.ComponentModel.DataAnnotations;

namespace SharedLib
{
    /// <summary>
    /// Конфигурация MQTT
    /// </summary>
    public class MqttConfigModel
    {
        /// <summary>
        /// Адрес сервера MQTT
        /// </summary>
        [Required]
        public string? Server { get; set; }

        /// <summary>
        /// Порт сервера MQTT
        /// </summary>
        [Required]
        public int Port { get; set; } = 8883;

        /// <summary>
        /// Максимальный размер пакета/сообщения MQTT
        /// </summary>
        [Required]
        public uint MessageMaxSizeBytes { get; set; } = 1000000;

        /// <summary>
        /// Логин пользователя для авторизации на сервере MQTT
        /// </summary>
        [Required]
        public string? Username { get; set; }

        /// <summary>
        /// Пароль пользователя для авторизации на сервере MQTT
        /// </summary>
        [Required]
        public string? Password { get; set; }

        /// <summary>
        /// Идентификатор
        /// </summary>
        [NotEqual(nameof(Username), ErrorMessage = "Логин должен отличаться от имени/идентификатора")]
        public string? ClientId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool AutoStart { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string? Secret { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsConfigured =>
            !string.IsNullOrWhiteSpace(Server) &&
            Port > 0 &&
            !string.IsNullOrWhiteSpace(Secret) &&
            !string.IsNullOrWhiteSpace(Username) &&
            !string.IsNullOrWhiteSpace(Password) &&
            !string.IsNullOrWhiteSpace(ClientId);

        /// <inheritdoc/>
        public static bool operator ==(MqttConfigModel l, MqttConfigModel r)
        {
            return
                l.AutoStart == r.AutoStart &&
                ((l.Server?.Equals(r.Server) == true) || (l.Server is null && r.Server is null)) &&
                l.Port == r.Port &&
                ((l.ClientId?.Equals(r.ClientId) == true) || (l.ClientId is null && r.ClientId is null)) &&
                l.MessageMaxSizeBytes == r.MessageMaxSizeBytes &&
                ((l.Username?.Equals(r.Username) == true) || (l.Username is null && r.Username is null)) &&
                ((l.Password?.Equals(r.Password) == true) || (l.Password is null && r.Password is null)) &&
                ((l.Secret?.Equals(r.Secret) == true) || (l.Secret is null && r.Secret is null));
        }

        /// <inheritdoc/>
        public static bool operator !=(MqttConfigModel l, MqttConfigModel r)
        {
            return !(l == r);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            return this == (MqttConfigModel)obj;
        }

        /// <inheritdoc/>
        public override int GetHashCode() => $"{Server}{Port}{Username}{Password}{Secret}{MessageMaxSizeBytes}{ClientId}{AutoStart}".GetHashCode();
    }
}