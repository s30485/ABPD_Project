using System.Text.RegularExpressions;

namespace ABPD_HW_02.Models;

public class EmbeddedDevice : Device
{
    private static readonly Regex IpRegex = new(@"^(\d{1,3}\.){3}\d{1,3}$");
    private string _ipAddress;
    public string IpAddress 
    { 
        get => _ipAddress;
        set
        {
            if (!IpRegex.IsMatch(value)) throw new ArgumentException("Invalid IP address format.");
            //matches the format with regexes, if the format is not findable then throws a ArgumentException
            _ipAddress = value;
        }
    }
    public string NetworkName { get; set; }
    
    public void Connect()
    {
        if (!NetworkName.Contains("MD Ltd.")) throw new ConnectionException();
        //many built-in functions like python, I like c# already
        //if NetworkName does not contain MD Ltd then throw exeption
        Console.WriteLine("Connected successfully.");
    }

    public override void TurnOn()
    {
        Connect();
        //used in turn on like task specifies
        IsTurnedOn = true;
    }

    public override string ToString() => $"Embedded Device [ID: {Id}, Name: {Name}, IP: {IpAddress}, Network: {NetworkName}, On: {IsTurnedOn}]";
}