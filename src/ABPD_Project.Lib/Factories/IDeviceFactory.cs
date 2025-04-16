using ABPD_HW_02.Models;

namespace ABPD_HW_02;

/// <summary>
/// Defines a contract for creating a device from a formatted string.
/// </summary>
public interface IDeviceFactory
{
    /// <summary>
    /// Creates a device based on the given string line input.
    /// </summary>
    /// <param name="line">The CSV-like string line describing a device.</param>
    /// <returns>A "Device" object or null if parsing fails.</returns>
    Device Create(string line);
}