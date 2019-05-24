using System.Collections.Generic;
using BinaryParkingClientServerNiverovskyi.Models.Vehicle;
using Newtonsoft.Json;

namespace BinaryParkingClientServerNiverovskyi.Models.Parking
{
    public class ParkingSettings
    {
        [JsonProperty("balance")] public double Balance { get; set; }
        [JsonProperty("paymentperiod")] public uint PaymentPeriod { get; set; }
        [JsonProperty("penaltyratio")] public double PenaltyRatio { get; set; }
        [JsonProperty("maxcapacity")] public uint MaxCapacity { get; set; }

        [JsonProperty("tariffs")]
        public Dictionary<VehicleType, double> Tariffs = new Dictionary<VehicleType, double>(4);
    }
}