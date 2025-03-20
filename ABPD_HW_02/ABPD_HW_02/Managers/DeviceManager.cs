using System.Text.RegularExpressions;
using ABPD_HW_02.Models;

namespace ABPD_HW_02.Managers;

public class DeviceManager
{
    private List<Device> _devices = new();
    private string _filePath;
    private string _outputFilePath = "C:\\Users\\Admin\\Desktop\\PJATK\\UNI_GIT_REPOS\\ABPD_HomeWorkProject\\ABPD_HW_02\\ABPD_HW_02\\resources\\output.txt";
    private const int MaxDevices = 15; //max count is 15
    
    public DeviceManager(string filePath)
    {
        _filePath = filePath;
        LoadDevices();
    }
    
    private void LoadDevices()
    {
        if (!File.Exists(_filePath)) return;//check if file exists
        foreach (var line in File.ReadLines(_filePath)) //built in static class File 
        {
            try
            {
                var parts = line.Split(',');
                if (parts.Length < 3) continue;
                
                var type = parts[0].Split('-');
                if (type.Length < 2) continue;

                if (type[0] == "SW")
                {
                    var smartWatch = new Smartwatch();
                    smartWatch.Id = int.Parse(type[1]);
                    smartWatch.Name = parts[1];
                    smartWatch.IsTurnedOn = bool.Parse(parts[2]);
                    smartWatch.BatteryPercentage = int.Parse(parts[3].Remove(parts[3].Length - 1));
                    _devices.Add(smartWatch);
                }
                else if (type[0] == "P")
                {
                    var personalComputer = new PersonalComputer();
                    personalComputer.Id = int.Parse(type[1]);
                    personalComputer.Name = parts[1];
                    personalComputer.IsTurnedOn = bool.Parse(parts[2]);
                    personalComputer.OperatingSystem = parts.Length == 3 ? "" : parts[3];
                    _devices.Add(personalComputer);
                }
                else if (type[0] == "ED")
                {
                    var embeddedDevice = new EmbeddedDevice();
                    embeddedDevice.Id = int.Parse(type[1]);
                    embeddedDevice.Name = parts[1];
                    embeddedDevice.IpAddress = parts[2];
                    embeddedDevice.NetworkName = parts[3];
                    _devices.Add(embeddedDevice);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing line: \"{line}\". Exception: {ex.Message}");
            }
        }
    }

    public void AddDevice(Device device)
    {
        if (_devices.Count >= MaxDevices) throw new InvalidOperationException("Device storage is full.");
        _devices.Add(device);
    }

    public void RemoveDevice(string deviceType, int id)
    {
        if (deviceType == "SW")
        {
            _devices.RemoveAll(d => d is Smartwatch sw && sw.Id == id);
        }
        else if (deviceType == "P")
        {
            _devices.RemoveAll(d => d is PersonalComputer pc && pc.Id == id);
        }
        else if (deviceType == "ED")
        {
            _devices.RemoveAll(d => d is EmbeddedDevice ed && ed.Id == id);
        }
        else
        {
            Console.WriteLine($"Unknown device type '{deviceType}'. Nothing removed.");
        }
    }

    public void EditDeviceData(string deviceType, int id, string property, object newValue) //I didn't find the materials in teams so I used youtube to learn boxing and unboxing, I don't know if its correctly implemented though
    //If I understood the youtube video correctly, boxing is turning a value type into an object type variable
    //unboxing is the opposite, so transforming a reference object type variable to a value type
    {
        Device device = null;
        if (deviceType == "SW")
        {
            device = _devices.FirstOrDefault(d => d is Smartwatch sw && sw.Id == id);
        }
        else if (deviceType == "P")
        {
            device = _devices.FirstOrDefault(d => d is PersonalComputer pc && pc.Id == id);
        }
        else if (deviceType == "ED")
        {
            device = _devices.FirstOrDefault(d => d is EmbeddedDevice ed && ed.Id == id);
        }

        if (device == null)
        {
            Console.WriteLine($"No {deviceType} device found with ID={id}.");
            return;
        }
        try
        {
            if (device is Smartwatch sw && property.Equals("Battery", StringComparison.OrdinalIgnoreCase))
            {
                sw.BatteryPercentage = (int)newValue;
            }
            else if (device is PersonalComputer pc && property.Equals("OS", StringComparison.OrdinalIgnoreCase))
            {
                pc.OperatingSystem = (string)newValue;
            }
            else if (device is EmbeddedDevice embDev)
            {
                if (property.Equals("IPAddress", StringComparison.OrdinalIgnoreCase))
                {
                    embDev.IpAddress = (string)newValue;
                }
                else if (property.Equals("NetworkName", StringComparison.OrdinalIgnoreCase))
                {
                    embDev.NetworkName = (string)newValue;
                }
                else
                {
                    Console.WriteLine($"Property '{property}' not recognized for EmbeddedDevice.");
                }
            }
            else
            {
                Console.WriteLine($"Property '{property}' not recognized or not applicable for {deviceType} with ID={id}.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error editing device data ({deviceType}-{id}). Reason: {ex.Message}");
        }
    }
    public void TurnOnDevice(string deviceType, int id)
    {
        Device device = null;
        if (deviceType == "SW")
        {
            device = _devices.FirstOrDefault(d => d is Smartwatch sw && sw.Id == id);
        }
        else if (deviceType == "P")
        {
            device = _devices.FirstOrDefault(d => d is PersonalComputer pc && pc.Id == id);
        }
        else if (deviceType == "ED")
        {
            device = _devices.FirstOrDefault(d => d is EmbeddedDevice embDev && embDev.Id == id);
        }

        if (device == null)
        {
            Console.WriteLine($"No {deviceType} device found with ID={id}.");
            return;
        }

        try
        {
            device.TurnOn();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not turn on {deviceType}-{id}. Reason: {ex.Message}");
        }
    }
    public void TurnOffDevice(string deviceType, int id) //turn off device with a given id
    {
        Device? device = null;

        if (deviceType == "SW")
        {
            device = _devices.FirstOrDefault(d => d is Smartwatch sw && sw.Id == id);
        }
        else if (deviceType == "P")
        {
            device = _devices.FirstOrDefault(d => d is PersonalComputer pc && pc.Id == id);
        }
        else if (deviceType == "ED")
        {
            device = _devices.FirstOrDefault(d => d is EmbeddedDevice embDev && embDev.Id == id);
        }

        if (device == null)
        {
            Console.WriteLine($"No {deviceType} device found with ID={id}.");
            return;
        }

        try
        {
            device.TurnOff();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not turn off {deviceType}-{id}. Reason: {ex.Message}");
        }
    }
    public void ShowAllDevices()
    {
        foreach (var device in _devices) Console.WriteLine(device);
    }
    
    public void SaveDevicesData()
    {
        var lines = _devices.Select(d =>
        {
            switch (d)
            {
                case Smartwatch sw:
                    return $"SW,{sw.Id},{sw.Name},{sw.IsTurnedOn},{sw.BatteryPercentage}";
                case PersonalComputer pc:
                    return $"P,{pc.Id},{pc.Name},{pc.IsTurnedOn},{pc.OperatingSystem}";
                case EmbeddedDevice ed:
                    return $"ED,{ed.Id},{ed.Name},{ed.IpAddress},{ed.NetworkName}";
                default:
                    return string.Empty;
            }
        }).ToList();

        File.WriteAllLines(_outputFilePath, lines);
        Console.WriteLine($"Devices saved to {_outputFilePath}");
    }
}