using Microsoft.EntityFrameworkCore;

namespace BinaryParkingClientServerNiverovskyi.Models.Vehicle
{
    public class VehicleContext : DbContext
    {
        public DbSet<Vehicle> Vehicles { get; set; }

        public VehicleContext(DbContextOptions options) : base(options)
        {
        }
    }
}