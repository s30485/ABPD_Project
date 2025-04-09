using System.Text.Json;
using ABPD_HW_02.Models;
using DeviceManager.RestAPI.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManager.RestAPI.Controllers
{
    /// <summary>
    /// Controller for managing devices via REST.
    /// </summary>
    [Route("api/devices")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly ABPD_HW_02.Managers.DeviceManager _deviceManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceController"/> class.
        /// </summary>
        /// <param name="deviceManager">The injected DeviceManager instance.</param>
        public DeviceController(ABPD_HW_02.Managers.DeviceManager deviceManager)
        {
            _deviceManager = deviceManager;
        }
        
        [HttpGet]
        public IActionResult GetAll()
        {
            var shortInfo = _deviceManager._devices.Select(d => new
            {
                d.Id,
                d.Name,
                Type = GetDeviceTypePrefix(d)
            });
            return Ok(shortInfo);
        }

        [HttpGet("{deviceType}/{id}")]
        public IActionResult GetById(string deviceType, int id)
        {
            var device = _deviceManager._devices
                .FirstOrDefault(d => d.Id == id && GetDeviceTypePrefix(d).Equals(deviceType, StringComparison.OrdinalIgnoreCase));
            if (device == null)
                return NotFound();
            return Ok(device);
        }

        /// <summary>
        /// Creates a new Smartwatch.
        /// </summary>
        [HttpPost("SW")]
        public IActionResult CreateSmartwatch([FromBody] SmartwatchRequest request)
        {
            try
            {
                var device = new Smartwatch
                {
                    //ID not set; AddDevice will auto-assign it -> changed the method in DeviceManager.cs in diferent project
                    Name = request.Name,
                    BatteryPercentage = request.BatteryPercentage
                };
                _deviceManager.AddDevice(device);
                return CreatedAtAction(nameof(GetById), new { deviceType = "SW", id = device.Id }, device);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Creates a new Personal Computer.
        /// </summary>
        [HttpPost("P")]
        public IActionResult CreatePersonalComputer([FromBody] PersonalComputerRequest request)
        {
            try
            {
                var device = new PersonalComputer
                {
                    Name = request.Name,
                    OperatingSystem = request.OperatingSystem
                };
                _deviceManager.AddDevice(device);
                return CreatedAtAction(nameof(GetById), new { deviceType = "P", id = device.Id }, device);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Creates a new Embedded Device.
        /// </summary>
        [HttpPost("ED")]
        public IActionResult CreateEmbeddedDevice([FromBody] EmbeddedDeviceRequest request)
        {
            try
            {
                var device = new EmbeddedDevice
                {
                    Name = request.Name,
                    IpAddress = request.IpAddress,
                    NetworkName = request.NetworkName
                };
                _deviceManager.AddDevice(device);
                return CreatedAtAction(nameof(GetById), new { deviceType = "ED", id = device.Id }, device);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing Smartwatch.
        /// </summary>
        [HttpPut("SW/{id}")]
        public IActionResult UpdateSmartwatch(int id, [FromBody] SmartwatchRequest request)
        {
            var existing = _deviceManager._devices.FirstOrDefault(d => d.Id == id && d is Smartwatch);
            if (existing == null)
                return NotFound();

            //remove the existing device and add the updated one.
            _deviceManager.RemoveDevice("SW", id);
            var device = new Smartwatch
            {
                Name = request.Name,
                BatteryPercentage = request.BatteryPercentage,
                Id = id  //preserve the original ID for update
            };
            _deviceManager.AddDevice(device);
            return NoContent();
        }

        /// <summary>
        /// Updates an existing Personal Computer.
        /// </summary>
        [HttpPut("P/{id}")]
        public IActionResult UpdatePersonalComputer(int id, [FromBody] PersonalComputerRequest request)
        {
            var existing = _deviceManager._devices.FirstOrDefault(d => d.Id == id && d is PersonalComputer);
            if (existing == null)
                return NotFound();

            _deviceManager.RemoveDevice("P", id);
            var device = new PersonalComputer
            {
                Name = request.Name,
                OperatingSystem = request.OperatingSystem,
                Id = id
            };
            _deviceManager.AddDevice(device);
            return NoContent();
        }

        /// <summary>
        /// Updates an existing Embedded Device.
        /// </summary>
        [HttpPut("ED/{id}")]
        public IActionResult UpdateEmbeddedDevice(int id, [FromBody] EmbeddedDeviceRequest request)
        {
            var existing = _deviceManager._devices.FirstOrDefault(d => d.Id == id && d is EmbeddedDevice);
            if (existing == null)
                return NotFound();

            _deviceManager.RemoveDevice("ED", id);
            var device = new EmbeddedDevice
            {
                Name = request.Name,
                IpAddress = request.IpAddress,
                NetworkName = request.NetworkName,
                Id = id
            };
            _deviceManager.AddDevice(device);
            return NoContent();
        }

        /// <summary>
        /// Deletes an existing device.
        /// </summary>
        [HttpDelete("{deviceType}/{id}")]
        public IActionResult Delete(string deviceType, int id)
        {
            var existing = _deviceManager._devices.FirstOrDefault(d =>
                d.Id == id && GetDeviceTypePrefix(d).Equals(deviceType, StringComparison.OrdinalIgnoreCase));
            if (existing == null)
                return NotFound();

            _deviceManager.RemoveDevice(deviceType, id);
            return NoContent();
        }

        /// <summary>
        /// Helper method to determine the device type prefix.
        /// </summary>
        private string GetDeviceTypePrefix(Device device)
        {
            if (device is Smartwatch) return "SW";
            if (device is PersonalComputer) return "P";
            if (device is EmbeddedDevice) return "ED";
            return string.Empty;
        }
    }
}