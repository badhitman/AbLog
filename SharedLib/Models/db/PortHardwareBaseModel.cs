﻿////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Порт устройства (базовая модель)
/// </summary>
public class PortHardwareBaseModel : EntryModel
{
    /// <summary>
    /// Номер порта
    /// </summary>
    public uint PortNum { get; set; }
}