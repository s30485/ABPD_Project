using System;
using System.IO;
using System.Linq;
using ABPD_HW_02.Factories;
using ABPD_HW_02.Managers;
using ABPD_HW_02.Models;
using JetBrains.Annotations;
using Xunit;

namespace ABPD_HW_02.Tests.Managers;

//to test: 
//constructor
//loadDevices
//addDevice
//removeDevice
//editDeviceData
//turnOnDevice
//turnOffDevice
//showAllDevices
//saveDevicesData

[TestSubject(typeof(DeviceManager))]
public class DeviceManagerTest
{
    private readonly string _testFilePath;
    private DeviceManager _deviceManager;

    public DeviceManagerTest()
    {
        //I need to do relative path but I have no clue how xdddddddd
        _testFilePath = "C:\\Users\\Admin\\Desktop\\PJATK\\UNI_GIT_REPOS\\ABPD_Project\\ABPD_Project\\ABPD_Project.Tests\\testResources\\test_input.txt";
        
        _deviceManager = DeviceManagerFactory.Create(_testFilePath, "dummy_output.txt");

    }

    [Fact]
    public void ConstructorTest()
    {
        _deviceManager = DeviceManagerFactory.Create(_testFilePath, "dummy_output.txt");
        
        int expectedDeviceCount = 5;
        
        Assert.Equal(_deviceManager._devices.Count, expectedDeviceCount);
    }
    
    
    [Fact]
    public void AddDeviceTest() //should add a device if storage not full
    {
        int initialCount = _deviceManager._devices.Count;
        
        var newPc = new PersonalComputer
        {
            Id = 999,
            Name = "TestPC",
            IsTurnedOn = false,
            OperatingSystem = "TestOS"
        };
        _deviceManager.AddDevice(newPc);

        // Assert
        int newCount = _deviceManager._devices.Count;
        Assert.Equal(initialCount + 1, newCount);
        Assert.Contains(_deviceManager._devices, d => d is PersonalComputer pc && pc.Id == 999);
    }
    
    [Fact]
    
    public void RemoveDeviceTest() //should remove specific device
    {
        int initialCount = _deviceManager._devices.Count;
        
        _deviceManager.RemoveDevice("SW", 1);
        
        int newCount = _deviceManager._devices.Count;
        Assert.True(newCount <= initialCount, "The count should decrease by 1 if the device existed.");
        Assert.DoesNotContain(_deviceManager._devices, d => d is Smartwatch sw && sw.Id == 1);
    }
    
    [Fact]
    
    public void EditDeviceData() //should change property for correct device
    {
        int targetId = 1;
        int newBatteryValue = 50;
        
        _deviceManager.EditDeviceData("SW", targetId, "Battery", newBatteryValue);
        
        var editedDevice = _deviceManager._devices
            .OfType<Smartwatch>()
            .FirstOrDefault(sw => sw.Id == targetId);
        //some indian dude used the .OfType<type>() on youtube, we did not use it I know, but I want to learn a new thing xd

        if (editedDevice != null)
        {
            Assert.Equal(newBatteryValue, editedDevice.BatteryPercentage);
        }
        else
        {
            Assert.True(false, $"No Smartwatch found with ID={targetId} to verify the edit.");
        }
    }
    
    [Fact]
    public void TurnOnDevice() //should turn device on
    {
        int targetId = 2;
        
        _deviceManager.TurnOnDevice("P", targetId);
        
        var turnedOnDevice = _deviceManager._devices
            .OfType<PersonalComputer>()
            .FirstOrDefault(pc => pc.Id == targetId);

        if (turnedOnDevice != null)
        {
            Assert.True(turnedOnDevice.IsTurnedOn, "Expected the PC to be turned on.");
        }
        else
        {
            Assert.Fail($"No PersonalComputer with ID={targetId} found to verify turn-on.");
        }
    }
    
    [Fact]
    public void TurnOffDevice() //should turn device off, almost the same as last test
    {
        int targetId = 1;
        _deviceManager.TurnOnDevice("P", targetId);
        
        _deviceManager.TurnOffDevice("P", targetId);
        
        var turnedOffDevice = _deviceManager._devices
            .OfType<PersonalComputer>()
            .FirstOrDefault(sw => sw.Id == targetId);

        if (turnedOffDevice != null)
        {
            Assert.False(turnedOffDevice.IsTurnedOn, "Expected the smartwatch to be turned off.");
        }
        else
        {
            Assert.Fail($"No Smartwatch with ID={targetId} found to verify turn-off.");
        }
    }
}