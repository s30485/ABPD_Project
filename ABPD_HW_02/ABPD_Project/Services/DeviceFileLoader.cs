using ABPD_HW_02.Models;

namespace ABPD_HW_02.Services;


/// <summary>
/// Loads devices from a file using a device factory.
/// </summary>
public class DeviceFileLoader : IDeviceLoader
{
    private readonly IDeviceFactory _factory;

    /// <summary>
    /// Initializes a new instance of the "DeviceFileLoader" class.
    /// </summary>
    /// <param name="factory"> The factory used to parse each line into a device.</param>
    public DeviceFileLoader(IDeviceFactory factory)
    {
        _factory = factory;
    }

    public List<Device> Load(string filePath)
    {
        var devices = new List<Device>();

        if (!File.Exists(filePath)) return devices;

        foreach (var line in File.ReadLines(filePath))
        {
            var device = _factory.Create(line);
            if (device != null)
                devices.Add(device);
        }

        return devices;
    }
}