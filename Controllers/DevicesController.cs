using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult GetAll()
        {
            devices.Clear();
            foreach (var device in db.Devices.ToList())
                devices.Add(device);

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
            if (id != device.Id)
            {
                return BadRequest();
            }
            var existingDevice = db.Devices.FirstOrDefault(u => u.Id == device.Id);
            if (existingDevice != null)
            {
                existingDevice.Name = device.Name;
                existingDevice.Price = device.Price;
                existingDevice.Manufacturer = device.Manufacturer;
                existingDevice.Quantity = device.Quantity;
                existingDevice.IsActive = device.IsActive;
                db.SaveChanges();
                return Ok(existingDevice);
            }
            return NotFound();
        }
        [HttpPatch("{id:int}")]
        public IActionResult Patch(int id, [FromBody] Device device)
        {
            if (id != device.Id)
            {
                return BadRequest();
            }
            var existingDevice = db.Devices.FirstOrDefault(u => u.Id == device.Id);
            if (existingDevice != null)
            {
                existingDevice.Name = device.Name;
                existingDevice.Price = device.Price;
                existingDevice.Manufacturer = device.Manufacturer;
                existingDevice.Quantity = device.Quantity;
                existingDevice.IsActive = device.IsActive;
                db.SaveChanges();
                return Ok(existingDevice);
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
