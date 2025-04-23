namespace ABPD_HW_02.Models;

/// <summary>
/// Factory class to instantiate specific types of devices based on string input.
/// </summary>
public class DeviceFactory : IDeviceFactory
{
    public Device Create(string line)
    {
        var parts = line.Split(',');
        if (parts.Length < 3) return null;

        // var type = parts[0].Split('-');
        // if (type.Length < 2) return null;

        var rawId = parts[0];
        if (string.IsNullOrWhiteSpace(rawId)) return null;

        try
        {
            // switch (type[0]) 
            
            switch (rawId.StartsWith("SW-") ? "SW"
                    : rawId.StartsWith("P-")  ? "P"
                    : rawId.StartsWith("ED-") ? "ED" 
                    : "")
            
            {
                case "SW":
                    return new Smartwatch
                    {
                        // Id = int.Parse(type[1]),
                        Id = rawId,
                        Name = parts[1],
                        IsTurnedOn = bool.Parse(parts[2]),
                        BatteryPercentage = int.Parse(parts[3].TrimEnd('%'))
                    };
                case "P":
                    return new PersonalComputer
                    {
                        // Id = int.Parse(type[1]),
                        Id = rawId,
                        Name = parts[1],
                        IsTurnedOn = bool.Parse(parts[2]),
                        OperatingSystem = parts.Length == 3 ? "" : parts[3]
                    };
                case "ED":
                    return new EmbeddedDevice
                    {
                        // Id = int.Parse(type[1]),
                        Id = rawId,
                        Name = parts[1],
                        IpAddress = parts[2],
                        NetworkName = parts[3]
                    };
                default:
                    return null;
            }
        }
        catch
        {
            return null;
        }
    }
}