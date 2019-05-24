using Newtonsoft.Json;
using System;

namespace BinaryParkingClientServerNiverovskyi.Models.Vehicle
{
    public class Vehicle
    {
        [JsonProperty("id")] public int Id { get; set; }
        [JsonProperty("isparked")] public bool IsParked { get; set; }
        [JsonProperty("plate")] public string Plate { get; set; }
        [JsonProperty("balance")] public double Balance { get; set; }
        [JsonProperty("lastpaidtime")] public DateTime LastPaidTime { get; set; }
        [JsonProperty("model")] public string Model { get; set; }
        [JsonProperty("typeofvehicle")] public VehicleType TypeOfVehicle { get; set; }
    }
}