using Newtonsoft.Json;
namespace BinaryParkingClientServerNiverovskyi.Models.Vehicle
{
    public enum VehicleType
    {
       [JsonProperty("passengercar")] PassengerCar,
        [JsonProperty("bus")]Bus,
        [JsonProperty("motorcycle")]Motorcycle,
        [JsonProperty("truck")]Truck
    }
}