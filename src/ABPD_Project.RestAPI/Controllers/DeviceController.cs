using System.Text.Json;
using ABPD_HW_02.Models;
using ABPD_Project.RestAPI.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace ABPD_Project.RestAPI.Controllers
{
    /// <summary>
    /// Controller for managing devices via REST endpoints.
    /// </summary>
    [Route("api/devices")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        /// <summary>
        /// Retrieves a summary list of all devices.
        /// </summary>
        /// <returns>An <see cref="IResult"/> containing a list of device IDs, names, and types.</returns>
        [HttpGet]
        public IResult GetAll()
        {
            var list = DeviceManager.Instance._devices
                        .Select(d => new
                        {
                            d.Id,
                            d.Name,
                            Type = GetDeviceTypePrefix(d)
                        });
            return Results.Ok(list);
        }

        /// <summary>
        /// Retrieves a single device by its string identifier.
        /// </summary>
        /// <param name="id">The string ID of the device (e.g. "SW-1").</param>
        /// <returns>
        /// 200 OK with the device object if found;  
        /// 404 Not Found otherwise.
        /// </returns>
        [HttpGet("{id}")]
        public IResult GetById(string id)
        {
            var device = DeviceManager.Instance._devices
                .FirstOrDefault(d => d.Id == id);

            return device is null
                ? Results.NotFound()
                : Results.Ok(device);
        }

        /// <summary>
        /// Creates a new <see cref="Smartwatch"/>.
        /// </summary>
        /// <param name="req">The details of the smartwatch to create.</param>
        /// <returns>
        /// 201 Created with a Location header pointing to <c>/api/devices/{id}</c> on success;  
        /// 400 Bad Request on validation or creation error.
        /// </returns>
        [HttpPost("smartwatches")]
        public IResult CreateSmartwatch([FromBody] SmartwatchRequest req)
        {
            try
            {
                var device = new Smartwatch
                {
                    Name = req.Name,
                    BatteryPercentage = req.BatteryPercentage
                };
                DeviceManager.Instance.AddDevice(device);

                return Results.Created($"/api/devices/{device.Id}", device);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing <see cref="Smartwatch"/>.
        /// </summary>
        /// <param name="id">The string ID of the smartwatch to update.</param>
        /// <param name="req">The new property values.</param>
        /// <returns>
        /// 204 No Content on successful update;  
        /// 404 Not Found if the device does not exist.
        /// </returns>
        [HttpPut("smartwatches/{id}")]
        public IResult UpdateSmartwatch(string id, [FromBody] SmartwatchRequest req)
        {
            var existing = DeviceManager.Instance._devices
                .FirstOrDefault(d => d.Id == id && d is Smartwatch);
            if (existing is null) return Results.NotFound();

            DeviceManager.Instance.RemoveDevice(id);
            var updated = new Smartwatch
            {
                Id = id,
                Name = req.Name,
                BatteryPercentage = req.BatteryPercentage
            };
            DeviceManager.Instance.AddDevice(updated);
            return Results.NoContent();
        }

        /// <summary>
        /// Creates a new <see cref="PersonalComputer"/>.
        /// </summary>
        /// <param name="req">The details of the PC to create.</param>
        /// <returns>
        /// 201 Created on success;  
        /// 400 Bad Request on error.
        /// </returns>
        [HttpPost("personalcomputers")]
        public IResult CreatePersonalComputer([FromBody] PersonalComputerRequest req)
        {
            try
            {
                var device = new PersonalComputer
                {
                    Name = req.Name,
                    OperatingSystem = req.OperatingSystem
                };
                DeviceManager.Instance.AddDevice(device);
                return Results.Created($"/api/devices/{device.Id}", device);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing <see cref="PersonalComputer"/>.
        /// </summary>
        /// <param name="id">The string ID of the PC to update.</param>
        /// <param name="req">The new property values.</param>
        /// <returns>
        /// 204 No Content on success;  
        /// 404 Not Found if not found.
        /// </returns>
        [HttpPut("personalcomputers/{id}")]
        public IResult UpdatePersonalComputer(string id, [FromBody] PersonalComputerRequest req)
        {
            var existing = DeviceManager.Instance._devices
                .FirstOrDefault(d => d.Id == id && d is PersonalComputer);
            if (existing is null) return Results.NotFound();

            DeviceManager.Instance.RemoveDevice(id);
            var updated = new PersonalComputer
            {
                Id = id,
                Name = req.Name,
                OperatingSystem = req.OperatingSystem
            };
            DeviceManager.Instance.AddDevice(updated);
            return Results.NoContent();
        }

        /// <summary>
        /// Creates a new <see cref="EmbeddedDevice"/>.
        /// </summary>
        /// <param name="req">The details of the embedded device to create.</param>
        /// <returns>
        /// 201 Created on success;  
        /// 400 Bad Request on error.
        /// </returns>
        [HttpPost("embeddeddevices")]
        public IResult CreateEmbeddedDevice([FromBody] EmbeddedDeviceRequest req)
        {
            try
            {
                var device = new EmbeddedDevice
                {
                    Name = req.Name,
                    IpAddress = req.IpAddress,
                    NetworkName = req.NetworkName
                };
                DeviceManager.Instance.AddDevice(device);
                return Results.Created($"/api/devices/{device.Id}", device);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing <see cref="EmbeddedDevice"/>.
        /// </summary>
        /// <param name="id">The string ID of the embedded device to update.</param>
        /// <param name="req">The new property values.</param>
        /// <returns>
        /// 204 No Content on success;  
        /// 404 Not Found if the device does not exist.
        /// </returns>
        [HttpPut("embeddeddevices/{id}")]
        public IResult UpdateEmbeddedDevice(string id, [FromBody] EmbeddedDeviceRequest req)
        {
            var existing = DeviceManager.Instance._devices
                .FirstOrDefault(d => d.Id == id && d is EmbeddedDevice);
            if (existing is null) return Results.NotFound();

            DeviceManager.Instance.RemoveDevice(id);
            var updated = new EmbeddedDevice
            {
                Id = id,
                Name = req.Name,
                IpAddress = req.IpAddress,
                NetworkName = req.NetworkName
            };
            DeviceManager.Instance.AddDevice(updated);
            return Results.NoContent();
        }

        /// <summary>
        /// Deletes a device by its string ID.
        /// </summary>
        /// <param name="id">The string ID of the device to delete.</param>
        /// <returns>
        /// 204 No Content on success;  
        /// 404 Not Found if the device does not exist.
        /// </returns>
        [HttpDelete("{id}")]
        public IResult Delete(string id)
        {
            var existing = DeviceManager.Instance._devices
                .FirstOrDefault(d => d.Id == id);
            if (existing is null) return Results.NotFound();

            DeviceManager.Instance.RemoveDevice(id);
            return Results.NoContent();
        }

        /// <summary>
        /// Determines the device type prefix for serialization in <see cref="GetAll"/>.
        /// </summary>
        /// <param name="d">The device instance.</param>
        /// <returns>A two-letter prefix: "SW", "P", or "ED".</returns>
        private string GetDeviceTypePrefix(Device d)
        {
            return d switch
            {
                Smartwatch => "SW",
                PersonalComputer => "P",
                EmbeddedDevice => "ED",
                _ => string.Empty
            };
        }
    }
}