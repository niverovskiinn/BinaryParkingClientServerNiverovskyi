using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BinaryParkingClientServerNiverovskyi.Models.Transaction
{
    public class Transaction
    {
        [JsonProperty("id")] public int Id { get; set; }
        [JsonProperty("datatime")] public DateTime _dateTime { get; set; }
        [JsonProperty("tariff")] public double _tariff { get; set; }
    }
}