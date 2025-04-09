using ABPD_HW_02.Models;
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

        /// <summary>
        /// Retrieves a list of devices (short info).
        /// </summary>
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

        /// <summary>
        /// Retrieves a specific device by device type and ID.
        /// </summary>
        /// <param name="deviceType">The device type prefix (e.g. "SW", "P", "ED").</param>
        /// <param name="id">The device ID.</param>
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
        /// Creates a new device.
        /// </summary>
        /// <param name="deviceType">The device type prefix for the new device.</param>
        /// <param name="device">The device information.</param>
        [HttpPost("{deviceType}")]
        public IActionResult Create(string deviceType, [FromBody] Device device)
        {
            // Ensure that the posted device matches the type provided in the route.
            if (GetDeviceTypePrefix(device) != deviceType)
            {
                return BadRequest($"Device type mismatch. Expected {deviceType}.");
            }

            try
            {
                _deviceManager.AddDevice(device);
                return CreatedAtAction(nameof(GetById), new { deviceType = deviceType, id = device.Id }, device);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing device.
        /// </summary>
        /// <param name="deviceType">The device type prefix.</param>
        /// <param name="id">The device ID.</param>
        /// <param name="updatedDevice">The updated device information.</param>
        [HttpPut("{deviceType}/{id}")]
        public IActionResult Update(string deviceType, int id, [FromBody] Device updatedDevice)
        {
            //ensure that the updated device's type matches the provided deviceType.
            if (GetDeviceTypePrefix(updatedDevice) != deviceType)
            {
                return BadRequest($"Device type mismatch. Expected {deviceType}.");
            }

            var existing = _deviceManager._devices.FirstOrDefault(d => d.Id == id &&
                GetDeviceTypePrefix(d).Equals(deviceType, StringComparison.OrdinalIgnoreCase));
            if (existing == null)
                return NotFound();

            try
            {
                //remove the old device and add the updated one.
                _deviceManager.RemoveDevice(deviceType, id);
                updatedDevice.Id = id;
                _deviceManager.AddDevice(updatedDevice);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes an existing device.
        /// </summary>
        /// <param name="deviceType">The device type prefix.</param>
        /// <param name="id">The device ID.</param>
        [HttpDelete("{deviceType}/{id}")]
        public IActionResult Delete(string deviceType, int id)
        {
            var existing = _deviceManager._devices.FirstOrDefault(d => d.Id == id &&
                GetDeviceTypePrefix(d).Equals(deviceType, StringComparison.OrdinalIgnoreCase));
            if (existing == null)
                return NotFound();

            _deviceManager.RemoveDevice(deviceType, id);
            return NoContent();
        }

        /// <summary>
        /// Helper method to determine the device type prefix.
        /// </summary>
        /// <param name="device">The device instance.</param>
        /// <returns>A string representing the device type prefix.</returns>
        private string GetDeviceTypePrefix(Device device)
        {
            if (device is Smartwatch) return "SW";
            if (device is PersonalComputer) return "P";
            if (device is EmbeddedDevice) return "ED";
            return string.Empty;
        }
    }
}