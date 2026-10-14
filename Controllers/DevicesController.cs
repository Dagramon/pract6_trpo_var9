using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using pract6.Models;

using System.Collections.ObjectModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace pract6.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {

        WebApiDatabaseContext db = new();
        public ObservableCollection<Device> devices { get; set; } = new();
        [HttpGet]
        public IActionResult GetAll([FromQuery] string? manufacture, [FromQuery] decimal? minPrice)
        {
            devices.Clear();
            foreach (var device in db.Devices.ToList())
            {

                if (!manufacture.IsNullOrEmpty() && device.Manufacturer != manufacture)
                    continue;
                if (minPrice >= 0 && device.Price <= minPrice)
                    continue;

                devices.Add(device);
            }

            if (devices.Count == 0)
                return NotFound();

            return Ok(devices);
        }
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            devices.Clear();
            foreach (var device in db.Devices.ToList())
                devices.Add(device);

            var foundDevice = devices.FirstOrDefault(u => u.Id == id);

            if (foundDevice != null)
                return Ok(foundDevice);
            else
                return NotFound();
        }
        [HttpPost]
        public IActionResult Create(Device device)
        {
            if (device != null)
            {
                if (device.Name == null || device.Price == null || device.Manufacturer == null || device.Quantity == null || device.IsActive == null)
                    return BadRequest();
                device.Id = 0;
                db.Devices.Add(device);
                db.SaveChanges();
                return Created();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPut("{id:int}")]
        public IActionResult Put(int id, [FromBody] Device device)
        {
            var existingDevice = db.Devices.FirstOrDefault(u => u.Id == id);
            if (existingDevice != null)
            {
                try
                {
                    if (device.Name == null || device.Price == null || device.Manufacturer == null || device.Quantity == null || device.IsActive == null)
                        return BadRequest();

                    existingDevice.Name = device.Name;
                    existingDevice.Price = device.Price;
                    existingDevice.Manufacturer = device.Manufacturer;
                    existingDevice.Quantity = device.Quantity;
                    existingDevice.IsActive = device.IsActive;
                    db.SaveChanges();
                    return NoContent();
                }
                catch
                {
                    return BadRequest();
                }
            }
            return NotFound();
        }
        [HttpPatch("{id:int}")]
        public IActionResult Patch(int id, [FromBody] Device device)
        {
            if (device == null)
                return BadRequest();

            var existingDevice = db.Devices.FirstOrDefault(u => u.Id == id);
            if (existingDevice != null)
            {
                if (device.Name != null)
                    existingDevice.Name = device.Name;
                if (device.Price != null)
                    existingDevice.Price = device.Price;
                if (device.Manufacturer != null)
                    existingDevice.Manufacturer = device.Manufacturer;
                if (device.Quantity != null)
                    existingDevice.Quantity = device.Quantity;
                if (device.IsActive != null)
                    existingDevice.IsActive = device.IsActive;
                db.SaveChanges();

                return NoContent();
            }
            return NotFound();
        }
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {

            devices.Clear();
            foreach (var device in db.Devices.ToList())
                devices.Add(device);

            var foundDevice = devices.FirstOrDefault(u => u.Id == id);
            if (foundDevice != null)
            {
                db.Devices.Remove(foundDevice);
                db.SaveChanges();
                return Ok();
            }
            else
                return NotFound();
        }
    }
}
