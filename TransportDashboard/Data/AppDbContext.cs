using Microsoft.EntityFrameworkCore;
using TransportDashboard.Data;
using TransportDashboard.Models;

namespace TransportDashboard.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<FreightShipment> Shipments => Set<FreightShipment>();
        public DbSet<Vehicle> Vehicles => Set<Vehicle>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }
    }
}