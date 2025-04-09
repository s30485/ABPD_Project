using System.Threading.Tasks.Dataflow;

namespace ABPD_HW_02;

[Serializable]
public class ConnectionException : Exception
{
    public ConnectionException() : base("Invalid network connection.") {} //base calls the constructor of the parent class, read that here -> https://stackoverflow.com/questions/2200241/in-c-sharp-how-do-i-define-my-own-exceptions
}