﻿////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Hardware пet HTTP request
/// </summary>
public class HardwareGetHttpRequestModel
{
    /// <summary>
    /// Hardware Id
    /// </summary>
    public int HardwareId { get; set; }

    /// <summary>
    /// Path
    /// </summary>
    public string? Path { get; set; }
}