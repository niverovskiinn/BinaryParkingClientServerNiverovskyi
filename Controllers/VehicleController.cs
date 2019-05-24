using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using BinaryParkingClientServerNiverovskyi.Models.Vehicle;

namespace BinaryParkingClientServerNiverovskyi.Controllers
{
    public class VehicleController : ControllerBase
    {
        private VehicleContext _vehicleContext;

        public VehicleController(VehicleContext vehicleContext)
        {
            _vehicleContext = vehicleContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Vehicle>> Get()
        {
            return _vehicleContext.Vehicles.ToList();
        }


        [HttpGet]
        public ActionResult<IEnumerable<Vehicle>> GetOne(int id)
        {
            var vehicle = _vehicleContext.Vehicles.FirstOrDefault(x => x.Id == id);
            if (vehicle == null)
                return NotFound();
            return new ObjectResult(vehicle);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Vehicle vehicle)
        {
            if (vehicle == null)
            {
                return BadRequest();
            }

            _vehicleContext.Vehicles.Add(vehicle);
            _vehicleContext.SaveChanges();
            return Ok(vehicle);
        }

        [HttpPut]
        public IActionResult Put([FromBody] Vehicle vehicle)
        {
            if (vehicle == null)
            {
                return BadRequest();
            }

            if (!_vehicleContext.Vehicles.Any(x => x.Id == vehicle.Id))
            {
                return NotFound();
            }

            _vehicleContext.Update(vehicle);
            _vehicleContext.SaveChanges();
            return Ok(vehicle);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var vehicle = _vehicleContext.Vehicles.FirstOrDefault(x => x.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            _vehicleContext.Vehicles.Remove(vehicle);
            _vehicleContext.SaveChanges();
            return Ok(vehicle);
        }

        [HttpGet]
        public IActionResult RechargeBalance(int id, double sum)
        {
            var vehicle = _vehicleContext.Vehicles.FirstOrDefault(x => x.Id == id);
            if (vehicle != null)
                return NotFound();

            vehicle.Balance += sum;
            _vehicleContext.Update(vehicle);
            _vehicleContext.SaveChanges();
            return Ok(vehicle);
        }
    }
}