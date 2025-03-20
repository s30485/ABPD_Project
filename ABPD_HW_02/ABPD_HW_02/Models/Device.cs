namespace ABPD_HW_02.Models;

public abstract class Device
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsTurnedOn { get; set; }
    
    public abstract void TurnOn(); //abstract because every device type does something a little bit different
    public void TurnOff() => IsTurnedOn = false;
    public override abstract string ToString();
}