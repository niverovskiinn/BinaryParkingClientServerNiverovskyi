namespace BinaryParkingClientServerNiverovskyi.Models.Parking
{
    public class Parking
    {
        public ParkingSettings ParkingSettings;
        public uint CurrentCapacity { get; set; }


//        private void TakeMoney()
//        {
//            while (true)
//            {
//                foreach (var vehicle in _vehicles)
//                {
//                    if ((DateTime.Now - vehicle.LastPaidTime).TotalSeconds < PaymentPeriod) continue;
//                    vehicle.LastPaidTime = DateTime.Now;
//                    var tariff = vehicle.Balance - Tariffs[vehicle.TypeOfVehicle] < 0
//                        ? Tariffs[vehicle.TypeOfVehicle] * PenaltyRatio
//                        : Tariffs[vehicle.TypeOfVehicle];
//                    vehicle.Balance -= tariff;
//                    Balance += tariff;
//                    new Transaction(vehicle.Plate, tariff);
//                }
//
//                Thread.Sleep(1000);
//            }
//        }
//
//        public static Parking GetInstance()
//        {
//            return _parking ?? (_parking = new Parking());
//        }
//
//        /// <summary>
//        /// Add Vehicle to the Parking.
//        /// Return true if there is a Parking place or Vehicle is already in the parking, else false.
//        /// </summary>
//        public bool Add(Vehicle vehicle)
//        {
//            if (CurrentCapacity >= MaxCapacity) return false;
//            if (vehicle.IsParked) return true;
//            _vehicles.Add(vehicle);
//            vehicle.IsParked = true;
//            ++CurrentCapacity;
//            return true;
//        }
//
//        /// <summary>
//        /// Remove Vehicle from the Parking.
//        /// Return true if the Vehicle was in the Parking, else false.
//        /// </summary>
//        public bool Remove(Vehicle vehicle)
//        {
//            if (!vehicle.IsParked) return false;
//            _vehicles.Remove(vehicle);
//            vehicle.IsParked = false;
//            --CurrentCapacity;
//            return true;
//        }
//
//        public void WriteAllVehicles()
//        {
//            Console.WriteLine($"Parking: {CurrentCapacity} vehicles");
//            if (CurrentCapacity <= 0) return;
//            for (var i = 0; i < _vehicles.Count; i++)
//            {
//                Console.WriteLine(
//                    $"{i + 1}. Type: {_vehicles[i].TypeOfVehicle.ToString()}\t Model: {_vehicles[i].Model}\t Plate: {_vehicles[i].Plate}\t Balance: {_vehicles[i].Balance}");
//            }
//        }
//
//        public uint FreePlaces()
//        {
//            return MaxCapacity - CurrentCapacity;
//        }
    }
}