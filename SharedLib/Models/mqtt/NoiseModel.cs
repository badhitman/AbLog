namespace SharedLib
{
    /// <summary>
    /// Цифровой шум
    /// </summary>
    public class NoiseModel
    {
        /// <summary>
        /// Цифровой шум
        /// </summary>
        public IEnumerable<string> Noise = new string[]
        {
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString()
        };
    }

    /// <summary>
    /// 
    /// </summary>
    public class SimpleIdNoiseModel: NoiseModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
    }
}
