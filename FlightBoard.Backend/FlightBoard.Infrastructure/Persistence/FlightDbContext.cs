using FlightBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlightBoard.Infrastructure.Persistence;

public class FlightDbContext : DbContext
{
    public FlightDbContext(DbContextOptions<FlightDbContext> options)
        : base(options) { }

    public DbSet<Flight> Flights => Set<Flight>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Flight>()
            .HasIndex(f => f.FlightNumber)
            .IsUnique();
    }
}
