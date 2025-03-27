using ABPD_HW_02.Models;

namespace ABPD_HW_02;

/// <summary>
/// Defines a contract for loading devices from a file.
/// </summary>
public interface IDeviceLoader
{
    /// <summary>
    /// Loads devices from the specified file path.
    /// </summary>
    /// <param name="filePath">The file path to load from.</param>
    /// <returns>A list of loaded "Device" objects.</returns>
    List<Device> Load(string filePath);
}