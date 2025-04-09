namespace DeviceManager.RestAPI.DataTransferObjects;

/// <summary>
/// Request DTO for creating or updating a Personal Computer.
/// </summary>
public class PersonalComputerRequest
{
    /// <summary>
    /// Gets or sets the name of the personal computer.
    /// </summary>
    public string Name { get; set; }
        
    /// <summary>
    /// Gets or sets the operating system of the computer.
    /// </summary>
    public string OperatingSystem { get; set; }
}