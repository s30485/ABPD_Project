using System.Text.Json;
using ABPD_HW_02.Models;
using ABPD_Project.RestAPI.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Controller for managing devices via REST.
    /// </summary>
    [Route("api/devices")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceController"/> class.
        /// </summary>
        /// <param name="deviceManager">The injected DeviceManager instance.</param>
        
        [HttpGet]
        public IActionResult GetAll()
        {
            var shortInfo = DeviceManager.Instance._devices.Select(d => new
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
            var device = DeviceManager.Instance._devices
                .FirstOrDefault(d => d.Id == id && GetDeviceTypePrefix(d).Equals(deviceType, StringComparison.OrdinalIgnoreCase));
            if (device == null)
                return NotFound();
            return Ok(device);
        }

        /// <summary>
        /// Creates a new Smartwatch.
        /// </summary>
        [HttpPost("SW")]
        public BadRequestObjectResult CreateSmartwatch([FromBody] SmartwatchRequest request)
        {
            try
            {
                var device = new Smartwatch
                {
                    //ID not set; AddDevice will auto-assign it -> changed the method in DeviceManager.cs in diferent project
                    Name = request.Name,
                    BatteryPercentage = request.BatteryPercentage
                };
                DeviceManager.Instance.AddDevice(device);
                return new BadRequestObjectResult(CreatedAtAction(nameof(GetById), new { deviceType = "SW", id = device.Id }, device));
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
                DeviceManager.Instance.AddDevice(device);
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
                DeviceManager.Instance.AddDevice(device);
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
            var existing = DeviceManager.Instance._devices.FirstOrDefault(d => d.Id == id && d is Smartwatch);
            if (existing == null)
                return NotFound();

            //remove the existing device and add the updated one.
            DeviceManager.Instance.RemoveDevice("SW", id);
            var device = new Smartwatch
            {
                Name = request.Name,
                BatteryPercentage = request.BatteryPercentage,
                Id = id  //preserve the original ID for update
            };
            DeviceManager.Instance.AddDevice(device);
            return NoContent();
        }

        /// <summary>
        /// Updates an existing Personal Computer.
        /// </summary>
        [HttpPut("P/{id}")]
        public IActionResult UpdatePersonalComputer(int id, [FromBody] PersonalComputerRequest request)
        {
            var existing = DeviceManager.Instance._devices.FirstOrDefault(d => d.Id == id && d is PersonalComputer);
            if (existing == null)
                return NotFound();

            DeviceManager.Instance.RemoveDevice("P", id);
            var device = new PersonalComputer
            {
                Name = request.Name,
                OperatingSystem = request.OperatingSystem,
                Id = id
            };
            DeviceManager.Instance.AddDevice(device);
            return NoContent();
        }

        /// <summary>
        /// Updates an existing Embedded Device.
        /// </summary>
        [HttpPut("ED/{id}")]
        public IActionResult UpdateEmbeddedDevice(int id, [FromBody] EmbeddedDeviceRequest request)
        {
            var existing = DeviceManager.Instance._devices.FirstOrDefault(d => d.Id == id && d is EmbeddedDevice);
            if (existing == null)
                return NotFound();

            DeviceManager.Instance.RemoveDevice("ED", id);
            var device = new EmbeddedDevice
            {
                Name = request.Name,
                IpAddress = request.IpAddress,
                NetworkName = request.NetworkName,
                Id = id
            };
            DeviceManager.Instance.AddDevice(device);
            return NoContent();
        }

        /// <summary>
        /// Deletes an existing device.
        /// </summary>
        [HttpDelete("{deviceType}/{id}")]
        public IActionResult Delete(string deviceType, int id)
        {
            var existing = DeviceManager.Instance._devices.FirstOrDefault(d =>
                d.Id == id && GetDeviceTypePrefix(d).Equals(deviceType, StringComparison.OrdinalIgnoreCase));
            if (existing == null)
                return NotFound();

            DeviceManager.Instance.RemoveDevice(deviceType, id);
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
