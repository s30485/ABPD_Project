namespace ABPD_HW_02;


/// <summary>
/// Defines a contract for notifying about low battery.
/// </summary>
public interface IPowerNotifier
{
    /// <summary>
    /// Notify about low battery.
    /// </summary>
    void NotifyLowBattery();
}