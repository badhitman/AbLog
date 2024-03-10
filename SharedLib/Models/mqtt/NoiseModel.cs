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
    public IEnumerable<string> Noise =
    [
        Guid.NewGuid().ToString(),
        Guid.NewGuid().ToString()
    ];
}