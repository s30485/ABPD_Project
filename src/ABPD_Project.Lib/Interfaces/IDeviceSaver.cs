using ABPD_HW_02.Models;

namespace ABPD_HW_02;

/// <summary>
/// Defines a contract for saving devices to a file.
/// </summary>
public interface IDeviceSaver
{
    /// <summary>
    /// Saves devices to the specified file path.
    /// </summary>
    /// <param name="outputPath">The output file path.</param>
    /// <param name="devices">The list of devices to save.</param>
    public void Save(string outputPath, List<Device> devices);
}