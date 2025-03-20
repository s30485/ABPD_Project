using ABPD_HW_02.Managers;
using ABPD_HW_02.Models;

namespace ABPD_HW_02;

class Program
{
    public static void Main(string[] args)
    {
            //path to the CSV-like file that holds device information.
            string filePath = "C:\\Users\\Admin\\Desktop\\PJATK\\UNI_GIT_REPOS\\ABPD_HomeWorkProject\\ABPD_HW_02\\ABPD_HW_02\\resources\\input.txt"; 
            
            //instantiate the DeviceManager, which should automatically loads devices from the file.
            var deviceManager = new DeviceManager(filePath);

            Console.WriteLine("Initial Device List (Loaded from file):");
            deviceManager.ShowAllDevices();
            Console.WriteLine();

            //AddDevice()
            var newSmartwatch = new Smartwatch { Id = 99, Name = "NewWatch", BatteryPercentage = 100 }; //id 99 is a smartwatch now
            deviceManager.AddDevice(newSmartwatch);
            Console.WriteLine("Added a new Smartwatch with ID=99.");
            deviceManager.ShowAllDevices();
            Console.WriteLine();

            //RemoveDevice()
            deviceManager.RemoveDevice("SW", 2);
            Console.WriteLine("Removed Smartwatch with ID=2 (if it existed).");
            deviceManager.ShowAllDevices();
            Console.WriteLine();

            //EditDeviceData()
            //here: change the battery of Smartwatch ID=99 from 100% to 50%.
            deviceManager.EditDeviceData("SW", 99, "Battery", 50);
            Console.WriteLine("Edited Smartwatch ID=99 battery to 50%.");
            deviceManager.ShowAllDevices();
            Console.WriteLine();
            
            //here: change a PC's Operating System if ID=1 is a PC
            deviceManager.EditDeviceData("P", 1, "OS", "Windows 11");
            Console.WriteLine("Edited PersonalComputer ID=1 OS to Windows 11.");
            deviceManager.ShowAllDevices();
            Console.WriteLine();

            //turn on a device
            //Smartwatch with ID=99
            deviceManager.TurnOnDevice("SW", 99);
            //PC with ID=1
            deviceManager.TurnOnDevice("P", 1);
            //EmbeddedDevice with ID=2 (if it exists)
            deviceManager.TurnOnDevice("ED", 2);
            Console.WriteLine("Attempted to turn on SW-99, P-1, and ED-2 (if ED-3 exists).");
            deviceManager.ShowAllDevices();
            Console.WriteLine();

            //turn off the Smartwatch with ID=99
            deviceManager.TurnOffDevice("SW", 99);
            Console.WriteLine("Turned off Smartwatch with ID=99.");
            deviceManager.ShowAllDevices();
            Console.WriteLine();

            //save updated data back to file
            deviceManager.SaveDevicesData();
            Console.WriteLine("Devices saved back to file.");
    }
}