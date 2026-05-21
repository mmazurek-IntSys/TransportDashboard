using Microsoft.EntityFrameworkCore;
using TransportDashboard.Models;

namespace TransportDashboard.Data;

public class TransportDbContext : DbContext
{
    public TransportDbContext(
        DbContextOptions<TransportDbContext> options)
        : base(options)
    {
    }

    public DbSet<Client> Clients => Set<Client>();

    public DbSet<FreightHistory> FreightHistories
        => Set<FreightHistory>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Client>()
            .HasMany(x => x.FreightHistories)
            .WithOne(x => x.Client)
            .HasForeignKey(x => x.ClientId);
    }
}