
namespace ABPD_Project.RestAPI.DataTransferObjects;

/// <summary>
/// Request DTO for creating or updating a Smartwatch.
/// </summary>
public class SmartwatchRequest
{
    /// <summary>
    /// Gets or sets the name of the smartwatch.
    /// </summary>
    public string Name { get; set; }
        
    /// <summary>
    /// Gets or sets the battery percentage of the smartwatch.
    /// </summary>
    public int BatteryPercentage { get; set; }
}