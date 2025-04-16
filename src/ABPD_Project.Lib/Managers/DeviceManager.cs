using System.Text.RegularExpressions;
using ABPD_HW_02;
using ABPD_HW_02.Models;

/// <summary>
/// Manages a collection of devices, allowing operations such as loading, modifying, and saving devices.
/// </summary>
public class DeviceManager
{
    public List<Device> _devices = new();
    private const int MaxDevices = 15;
    private readonly IDeviceSaver _deviceSaver;
    private readonly string _outputFilePath;
    private static DeviceManager? _instance = null;

    public static DeviceManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new DeviceManager();
            return _instance;
        }
    }

    /// <summary>
    /// Initializes a new instance of the "DeviceManager" class.
    /// </summary>
    /// <param name="devices">Initial list of devices to manage.</param>
    /// <param name="deviceSaver">An implementation of "IDeviceSaver" for saving devices.</param>
    /// <param name="outputFilePath">Path to save updated device data.</param>
    public DeviceManager(List<Device> devices, IDeviceSaver deviceSaver, string outputFilePath)
    {
        _devices = devices ?? new();
        _deviceSaver = deviceSaver;
        _outputFilePath = outputFilePath;
    }

    /// <summary>
    /// Adds a device to the internal device list.
    /// </summary>
    /// <param name="device">The device to add.</param>
    /// <exception cref="InvalidOperationException">Thrown when the maximum number of devices is exceeded.</exception>
    public void AddDevice(Device device)
    {
        if (_devices.Count >= MaxDevices)
            throw new InvalidOperationException("Device storage is full.");

        // If no ID was provided (equals 0), assign a new one based on device type
        if (device.Id == 0)
        {
            int newId = _devices
                .Where(d => d.GetType() == device.GetType())
                .Select(d => d.Id)
                .DefaultIfEmpty(0)
                .Max() + 1;

            device.Id = newId;
        }

        _devices.Add(device);
    }


    /// <summary>
    /// Removes a device by type and ID.
    /// </summary>
    /// <param name="deviceType">Device type identifier ("SW", "P", "ED", ...).</param>
    /// <param name="id">ID of the device to remove.</param>
    public void RemoveDevice(string deviceType, int id)
    {
        _devices.RemoveAll(d =>
            (deviceType == "SW" && d is Smartwatch sw && sw.Id == id) ||
            (deviceType == "P" && d is PersonalComputer pc && pc.Id == id) ||
            (deviceType == "ED" && d is EmbeddedDevice ed && ed.Id == id));
    }

    /// <summary>
    /// Edits a property of a device.
    /// </summary>
    /// <param name="deviceType">Device type identifier ("SW", "P", "ED", ...).</param>
    /// <param name="id">ID of the device to modify.</param>
    /// <param name="property">The name of the property to edit ("Battery", "OS", ...).</param>
    /// <param name="newValue">The new value to set.</param>
    public void EditDeviceData(string deviceType, int id, string property, object newValue)
    {
        var device = _devices.FirstOrDefault(d =>
            (deviceType == "SW" && d is Smartwatch sw && sw.Id == id) ||
            (deviceType == "P" && d is PersonalComputer pc && pc.Id == id) ||
            (deviceType == "ED" && d is EmbeddedDevice ed && ed.Id == id));

        if (device == null)
        {
            Console.WriteLine($"No {deviceType} device found with ID={id}.");
            return;
        }

        try
        {
            switch (device)
            {
                case Smartwatch sw when property.Equals("Battery", StringComparison.OrdinalIgnoreCase):
                    sw.BatteryPercentage = (int)newValue;
                    break;
                case PersonalComputer pc when property.Equals("OS", StringComparison.OrdinalIgnoreCase):
                    pc.OperatingSystem = (string)newValue;
                    break;
                case EmbeddedDevice ed:
                    if (property.Equals("IPAddress", StringComparison.OrdinalIgnoreCase))
                        ed.IpAddress = (string)newValue;
                    else if (property.Equals("NetworkName", StringComparison.OrdinalIgnoreCase))
                        ed.NetworkName = (string)newValue;
                    break;
                default:
                    Console.WriteLine($"Invalid property for {deviceType}.");
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error editing device: {ex.Message}");
        }
    }

    /// <summary>
    /// Turns on a device by type and ID.
    /// </summary>
    /// <param name="deviceType">Device type identifier ("SW", "P", "ED", ...).</param>
    /// <param name="id">ID of the device to turn on.</param>
    public void TurnOnDevice(string deviceType, int id)
        => _devices.FirstOrDefault(d => MatchDevice(d, deviceType, id))?.TurnOn();

    /// <summary>
    /// Turns off a device by type and ID.
    /// </summary>
    /// <param name="deviceType">Device type identifier ("SW", "P", "ED", ...).</param>
    /// <param name="id">ID of the device to turn off.</param>
    public void TurnOffDevice(string deviceType, int id)
        => _devices.FirstOrDefault(d => MatchDevice(d, deviceType, id))?.TurnOff();

    /// <summary>
    /// Displays all devices to the console.
    /// </summary>
    public void ShowAllDevices()
    {
        Console.WriteLine("All devices:");
        foreach (var device in _devices)
            Console.WriteLine(device);
    }

    /// <summary>
    /// Saves all device data to the configured output file.
    /// </summary>
    public void SaveDevicesData()
    {
        _deviceSaver.Save(_outputFilePath, _devices);
    }

    private bool MatchDevice(Device d, string type, int id)
        => (type == "SW" && d is Smartwatch sw && sw.Id == id) ||
           (type == "P" && d is PersonalComputer pc && pc.Id == id) ||
           (type == "ED" && d is EmbeddedDevice ed && ed.Id == id);
}
