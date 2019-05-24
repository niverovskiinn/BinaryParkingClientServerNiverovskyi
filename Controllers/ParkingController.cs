using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BinaryParkingClientServerNiverovskyi.Models.Vehicle;
using BinaryParkingClientServerNiverovskyi.Models.Parking;
using BinaryParkingClientServerNiverovskyi.Models.Transaction;

namespace BinaryParkingClientServerNiverovskyi.Controllers
{
    public class ParkingController : ControllerBase
    {
        private List<Vehicle> _vehicles = new List<Vehicle>();

        private Parking _parking = new Parking
        {
            CurrentCapacity = 0,
            ParkingSettings = new ParkingSettings
            {
                PaymentPeriod = 5,
                PenaltyRatio = 2.5,
                MaxCapacity = 10,
                Balance = 0,
                Tariffs =
                {
                    {VehicleType.Bus, 3.5},
                    {VehicleType.PassengerCar, 2},
                    {VehicleType.Motorcycle, 1},
                    {VehicleType.Truck, 5}
                }
            }
        };

        public ParkingController()
        {
            new Thread(TakeMoney).Start();
        }

        private void TakeMoney()
        {
            while (true)
            {
                foreach (var vehicle in _vehicles)
                {
                    if ((DateTime.Now - vehicle.LastPaidTime).TotalSeconds <
                        _parking.ParkingSettings.PaymentPeriod) continue;
                    vehicle.LastPaidTime = DateTime.Now;
                    var tariff = vehicle.Balance - _parking.ParkingSettings.Tariffs[vehicle.TypeOfVehicle] < 0
                        ? _parking.ParkingSettings.Tariffs[vehicle.TypeOfVehicle] *
                          _parking.ParkingSettings.PenaltyRatio
                        : _parking.ParkingSettings.Tariffs[vehicle.TypeOfVehicle];
                    vehicle.Balance -= tariff;
                    _parking.ParkingSettings.Balance += tariff;
                    TransactionController.Add(new Transaction
                    {
                        _dateTime = DateTime.Now,
                        _tariff = tariff,
                        Id = vehicle.Id
                    });
                }

                Thread.Sleep(1000);
            }
        }

        [HttpPost]
        public ActionResult<IEnumerable<Vehicle>> EditSettings([FromBody] ParkingSettings parkingSettings)
        {
            if (parkingSettings == null)
                return BadRequest();
            _parking.ParkingSettings = parkingSettings;
            return Ok(parkingSettings);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Vehicle>> GetAllVehicles()
        {
            return _vehicles;
        }

        [HttpGet]
        public double Balance()
        {
            return _parking.ParkingSettings.Balance;
        }

        [HttpGet]
        public (uint, uint) PlacesFreeEngaged()
        {
            return (_parking.ParkingSettings.MaxCapacity - _parking.CurrentCapacity, _parking.CurrentCapacity);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Vehicle>> GetOne(int id)
        {
            var vehicle = _vehicles.FirstOrDefault(x => x.Id == id);
            if (vehicle == null)
                return NotFound();
            return new ObjectResult(vehicle);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Vehicle vehicle)
        {
            if (vehicle == null)
                return BadRequest();

            _vehicles.Add(vehicle);
            return Ok(vehicle);
        }


        [HttpDelete]
        public IActionResult Remove(int id)
        {
            var vehicle = _vehicles.FirstOrDefault(x => x.Id == id);
            if (vehicle == null)
                return NotFound();

            _vehicles.Remove(vehicle);
            return Ok(vehicle);
        }

        [HttpGet]
        public IActionResult RechargeBalance(int id, double sum)
        {
            var vehicle = _vehicles.FirstOrDefault(x => x.Id == id);
            if (vehicle == null)
                return NotFound();
            vehicle.Balance += sum;
            return Ok(vehicle);
        }
    }
}