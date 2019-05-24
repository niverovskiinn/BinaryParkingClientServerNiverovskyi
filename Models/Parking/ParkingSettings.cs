using System.Collections.Generic;
using BinaryParkingClientServerNiverovskyi.Models.Vehicle;

namespace BinaryParkingClientServerNiverovskyi.Models.Parking
{
    public class ParkingSettings
    {
        public double Balance { get; set; }
        public uint PaymentPeriod { get; set; }
        public double PenaltyRatio { get; set; }
        public uint MaxCapacity { get; set; }
        public Dictionary<VehicleType, double> Tariffs = new Dictionary<VehicleType, double>(4);
    }
}