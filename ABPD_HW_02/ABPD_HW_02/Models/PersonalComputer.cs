namespace ABPD_HW_02.Models;

public class PersonalComputer : Device
{
    public string OperatingSystem { get; set; }
    
    public override void TurnOn()
    {
        if (string.IsNullOrEmpty(OperatingSystem)) throw new EmptySystemException();
        //if no operating system -> throw exception
        IsTurnedOn = true;
    }

    public override string ToString() => $"PC [ID: {Id}, Name: {Name}, OS: {OperatingSystem ?? "None"}, On: {IsTurnedOn}]";
}