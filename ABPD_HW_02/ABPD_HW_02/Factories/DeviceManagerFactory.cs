using ABPD_HW_02.Managers;
using ABPD_HW_02.Models;
using ABPD_HW_02.Services;

namespace ABPD_HW_02.Factories;


/// <summary>
/// Factory responsible for creating "DeviceManager" instances.
/// </summary>
public static class DeviceManagerFactory
{
    /// <summary>
    /// Creates and initializes a new "DeviceManager" with loaded devices.
    /// </summary>
    /// <param name="inputFilePath"> The path to the input file for loading devices.</param>
    /// <param name="outputFilePath"> The path to the output file for saving devices.</param>
    /// <returns>A fully initialized "DeviceManager".</returns>
    public static DeviceManager Create(string inputFilePath, string outputFilePath)
    {
        var factory = new DeviceFactory();
        var loader = new DeviceFileLoader(factory);
        var saver = new DeviceFileSaver();
        var devices = loader.Load(inputFilePath);

        return new DeviceManager(devices, saver, outputFilePath);
    }
}