////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

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