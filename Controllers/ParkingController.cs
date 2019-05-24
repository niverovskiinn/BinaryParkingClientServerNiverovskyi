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
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingController : ControllerBase
    {
        private static List<Vehicle> _vehicles;

        private static Parking _parking;

        public static int ID = 1;
        private static Thread _thread;

        public ParkingController()
        {
            _vehicles = _vehicles ?? new List<Vehicle>();
            _parking = _parking ?? new Parking
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
            _thread = _thread ?? new Thread(TakeMoney);
            if (!_thread.IsAlive)
                _thread.Start();
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

        [HttpGet("EditSettings")]
        public ActionResult EditSettings(double balance, uint paymentperiod, double penalty, uint maxc, int bust,
            int truckt, int cart, int motot)
        {
            _parking.ParkingSettings.PaymentPeriod = paymentperiod;
            _parking.ParkingSettings.PenaltyRatio = penalty;
            _parking.ParkingSettings.MaxCapacity = maxc;
            _parking.ParkingSettings.Balance = balance;
            _parking.ParkingSettings.Tariffs[VehicleType.Bus] = bust;
            _parking.ParkingSettings.Tariffs[VehicleType.PassengerCar] = cart;
            _parking.ParkingSettings.Tariffs[VehicleType.Motorcycle] = motot;
            _parking.ParkingSettings.Tariffs[VehicleType.Truck] = truckt;
            return Ok();
        }

        [HttpGet("GetAllVehicles")]
        public ActionResult<IEnumerable<Vehicle>> GetAllVehicles()
        {
            return _vehicles;
        }

        [HttpGet("Balance")]
        public double Balance()
        {
            return _parking.ParkingSettings.Balance;
        }

        [HttpGet("MinId")]
        public int MinId()
        {
            return _vehicles.Min(vehicle => vehicle.Id);
        }

        [HttpGet("MaxId")]
        public int MaxId()
        {
            return _vehicles.Max(vehicle => vehicle.Id);
        }


        [HttpGet("PlacesFree")]
        public uint PlacesFree()
        {
            return _parking.ParkingSettings.MaxCapacity - _parking.CurrentCapacity;
        }

        [HttpGet("PlacesEngaged")]
        public uint PlacesEngaged()
        {
            return _parking.CurrentCapacity;
        }


        [HttpGet("GetOne")]
        public ActionResult<IEnumerable<Vehicle>> GetOne(int id)
        {
            var vehicle = _vehicles.FirstOrDefault(x => x.Id == id);
            if (vehicle == null)
                return NotFound();
            return new ObjectResult(vehicle);
        }

        [HttpGet("Add")]
        public IActionResult Add(int type, int balance)
        {
            _vehicles.Add(new Vehicle
            {
                Id = ID++,
                TypeOfVehicle = (VehicleType) type,
                Balance = balance,
                LastPaidTime = DateTime.Now
            });
            _parking.CurrentCapacity++;
            return Ok();
        }


        [HttpGet("Remove")]
        public IActionResult Remove(int id)
        {
            var vehicle = _vehicles.FirstOrDefault(x => x.Id == id);
            if (vehicle == null)
                return NotFound();
            _vehicles.Remove(vehicle);
            _parking.CurrentCapacity--;
            return Ok(vehicle);
        }

        [HttpGet("RechargeBalance")]
        public IActionResult RechargeBalance(int id, double sum)
        {
            var vehicle = _vehicles.FirstOrDefault(x => x.Id == id);
            if (vehicle == null)
                return NotFound();
            vehicle.Balance += sum;
            return Ok(vehicle);
        }

        [HttpGet("Quit")]
        public IActionResult Quit()
        {
            _thread.Abort();

            return Ok();
        }
    }
}