using System;
using System.IO;
using ABPD_HW_02.Managers;
using ABPD_HW_02.Models;
using JetBrains.Annotations;
using Xunit;

namespace ABPD_HW_02.Tests.Managers;

[TestSubject(typeof(DeviceManager))]
public class DeviceManagerTest
{
    private readonly string _testFilePath;
    private DeviceManager _deviceManager;

    public DeviceManagerTest()
    {
        //I need to do relative path but I have no clue how xdddddddd
        _testFilePath = "C:\\Users\\Admin\\Desktop\\PJATK\\UNI_GIT_REPOS\\ABPD_Project\\ABPD_HW_02\\ABPD_HW_02.Tests\\testResources\\test_input.txt";
        
        _deviceManager = new DeviceManager(_testFilePath);
    }

    [Fact]
    public void ConstructorTest()
    {
        
    }
    
    
    [Fact]
    public void Constructor_ShouldLoadDevices_FromExistingFile()
    {
        string inputFilePath = _testFilePath;
        
        var manager = new DeviceManager(inputFilePath);
        
        
        Assert.True(manager != null, "DeviceManager should not be null.");
        
    }

}