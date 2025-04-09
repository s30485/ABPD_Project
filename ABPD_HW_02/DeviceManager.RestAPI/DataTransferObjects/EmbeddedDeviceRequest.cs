namespace DeviceManager.RestAPI.DataTransferObjects;

/// <summary>
/// Request DTO for creating or updating an Embedded Device.
/// </summary>
public class EmbeddedDeviceRequest
{
    /// <summary>
    /// Gets or sets the name of the embedded device.
    /// </summary>
    public string Name { get; set; }
        
    /// <summary>
    /// Gets or sets the IP address of the device.
    /// </summary>
    public string IpAddress { get; set; }
        
    /// <summary>
    /// Gets or sets the network name of the device.
    /// </summary>
    public string NetworkName { get; set; }
}