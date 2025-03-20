namespace ABPD_HW_02.Models;

public class Smartwatch : Device, IPowerNotifier
{
    private int _batteryPercentage; //private fields start from _
    public int BatteryPercentage 
    { 
        get => _batteryPercentage;
        set
        {
            if (value < 0 || value > 100) throw new ArgumentOutOfRangeException("Battery percentage must be between 0 and 100.");
            _batteryPercentage = value;
            if (_batteryPercentage < 20) NotifyLowBattery();
        }
    }

    public void NotifyLowBattery() => Console.WriteLine("Warning: Battery is low!");

    public override void TurnOn()
    {
        if (BatteryPercentage < 11) throw new EmptyBatteryException();
        IsTurnedOn = true;
        BatteryPercentage -= 10;
    }

    public override string ToString() => $"Smartwatch [ID: {Id}, Name: {Name}, Battery: {BatteryPercentage}%, On: {IsTurnedOn}]";
}