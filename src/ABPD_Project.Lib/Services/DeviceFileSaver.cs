using ABPD_HW_02.Models;

namespace ABPD_HW_02.Services;

/// <summary>
/// Saves devices to a text file in a CSV-like format.
/// </summary>
public class DeviceFileSaver : IDeviceSaver
{
    public void Save(string outputPath, List<Device> devices)
    {
        var lines = devices.Select(d =>
        {
            return d switch
            {
                Smartwatch sw => $"{sw.Id},{sw.Name},{sw.IsTurnedOn},{sw.BatteryPercentage}%",
                PersonalComputer pc => $"{pc.Id},{pc.Name},{pc.IsTurnedOn},{pc.OperatingSystem}",
                EmbeddedDevice ed => $"{ed.Id},{ed.Name},{ed.IpAddress},{ed.NetworkName}",
                _ => string.Empty
            };
        }).ToList();

        File.WriteAllLines(outputPath, lines);
        Console.WriteLine($"Devices saved to {outputPath}");
    }
}