namespace SharedLib
{
    /// <summary>
    /// 
    /// </summary>
    public class MqttConfigModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string? Server { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Port { get; set; } = 8883;

        /// <summary>
        /// 
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long MessageMaxSizeBytes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? ClientId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool AutoStart { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsConfigured =>
            !string.IsNullOrWhiteSpace(Username) &&
            !string.IsNullOrWhiteSpace(Server) &&
            Port > 0 &&
            !string.IsNullOrWhiteSpace(Password) &&
            !string.IsNullOrWhiteSpace(ClientId);

        /// <inheritdoc/>
        public static bool operator ==(MqttConfigModel l, MqttConfigModel r)
        {
            return
                l.AutoStart == r.AutoStart &&
                ((l.Server?.Equals(r.Server) == true) || (l.Server is null && r.Server is null)) &&
                ((l.ClientId?.Equals(r.ClientId) == true) || (l.ClientId is null && r.ClientId is null)) &&
                l.MessageMaxSizeBytes == r.MessageMaxSizeBytes &&
                ((l.Username?.Equals(r.Username) == true) || (l.Username is null && r.Username is null)) &&
                l.Port == r.Port &&
                ((l.Password?.Equals(r.Password) == true) || (l.Password is null && r.Password is null));
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
        public override int GetHashCode() => $"{this.Server}{this.Port}{this.Username}{this.Password}{this.MessageMaxSizeBytes}{this.ClientId}{this.AutoStart}".GetHashCode();
    }
}