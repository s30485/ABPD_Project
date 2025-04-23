using System.Text.RegularExpressions;
using ABPD_HW_02;
using ABPD_HW_02.Factories;
using ABPD_HW_02.Models;
using ABPD_HW_02.Services;

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

    private DeviceManager()
    { 
        const string inputFilePath  = "resources/input.txt";
        const string outputFilePath = "devices_output.txt";

        // delegate to DeviceManagerFactory
        var real = DeviceManagerFactory.Create(inputFilePath, outputFilePath);

        // copy over everything
        _devices = real._devices;
        _deviceSaver = real._deviceSaver;
        _outputFilePath = real._outputFilePath;
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

        // If no ID was provided, generate one like "SW-1", "P-2", "ED-3", etc.
        if (string.IsNullOrWhiteSpace(device.Id))
        {
            // Determine the prefix by device type
            var prefix = device switch
            {
                Smartwatch       => "SW",
                PersonalComputer => "P",
                EmbeddedDevice   => "ED",
                _ => throw new InvalidOperationException("Unknown device type.")
            };

            // Find existing suffix numbers for this prefix
            var maxSuffix = _devices
                .Where(d => d.Id.StartsWith(prefix + "-"))
                .Select(d => d.Id.Substring(prefix.Length + 1))
                .Select(s => int.TryParse(s, out var n) ? n : 0)
                .DefaultIfEmpty(0)
                .Max();

            // Assign next available
            device.Id = $"{prefix}-{maxSuffix + 1}";
        }

        _devices.Add(device);
    }


    /// <summary>
    /// Removes a device by type and ID.
    /// </summary>
    /// <param name="deviceType">Device type identifier ("SW", "P", "ED", ...).</param>
    /// <param name="id">ID of the device to remove.</param>
    public void RemoveDevice(string id) //id now a string value
    {
        _devices.RemoveAll(d => d.Id == id);
    }

    /// <summary>
    /// Edits a property of a device.
    /// </summary>
    /// <param name="deviceType">Device type identifier ("SW", "P", "ED", ...).</param>
    /// <param name="id">ID of the device to modify.</param>
    /// <param name="property">The name of the property to edit ("Battery", "OS", ...).</param>
    /// <param name="newValue">The new value to set.</param>
    public void EditDeviceData(string id, string property, object newValue)
    {
        var device = _devices.FirstOrDefault(d => d.Id == id);
        if (device == null)
        {
            Console.WriteLine($"No device found with ID={id}.");
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
                    Console.WriteLine($"Invalid property for device ID={id}.");
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
    
    public void TurnOnDevice(string id)
        => _devices.FirstOrDefault(d => d.Id == id)?.TurnOn();
    
    /// <summary>
    /// Turns off a device by type and ID.
    /// </summary>
    /// <param name="deviceType">Device type identifier ("SW", "P", "ED", ...).</param>
    /// <param name="id">ID of the device to turn off.</param>
    public void TurnOffDevice(string id)
        => _devices.FirstOrDefault(d => d.Id == id)?.TurnOff();


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

    // private bool MatchDevice(Device d, string type, int id)
    //     => (type == "SW" && d is Smartwatch sw && sw.Id == id) ||
    //        (type == "P" && d is PersonalComputer pc && pc.Id == id) ||
    //        (type == "ED" && d is EmbeddedDevice ed && ed.Id == id);
}
