using Xunit;
using Microsoft.EntityFrameworkCore;
using FlightBoard.Infrastructure.Persistence;
using FlightBoard.Domain.Entities;
using FlightBoard.Application.Interfaces;
using System.Threading.Tasks;

public class FlightRepositoryTests
{
    private FlightDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<FlightDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new FlightDbContext(options);
    }

    [Fact]
    public async Task Add_And_Get_Flight_Works()
    {
        using var context = CreateContext();
        IFlightRepository repo = new FlightRepository(context);
        var flight = new Flight { Id = Guid.NewGuid(), FlightNumber = "LY100", Destination = "Tokyo", Gate = "A2", DepartureTime = DateTime.UtcNow.AddHours(2) };
        await repo.AddFlightAsync(flight);
        var all = await repo.GetAllFlightsAsync();
        Assert.Single(all);
        Assert.Equal("LY100", all[0].FlightNumber);
    }

    [Fact]
    public async Task Delete_Flight_Removes_It()
    {
        using var context = CreateContext();
        IFlightRepository repo = new FlightRepository(context);
        var flight = new Flight { Id = Guid.NewGuid(), FlightNumber = "LY200", Destination = "LA", Gate = "B1", DepartureTime = DateTime.UtcNow.AddHours(3) };
        await repo.AddFlightAsync(flight);
        await repo.DeleteFlightAsync(flight.Id);
        var all = await repo.GetAllFlightsAsync();
        Assert.Empty(all);
    }

    [Fact]
    public async Task FlightNumberExistsAsync_Returns_True_If_Exists()
    {
        using var context = CreateContext();
        IFlightRepository repo = new FlightRepository(context);
        var flight = new Flight { Id = Guid.NewGuid(), FlightNumber = "LY300", Destination = "Paris", Gate = "C1", DepartureTime = DateTime.UtcNow.AddHours(4) };
        await repo.AddFlightAsync(flight);
        var exists = await repo.FlightNumberExistsAsync("LY300");
        Assert.True(exists);
    }

    [Fact]
    public async Task GetFlightByIdAsync_Returns_Flight_If_Exists()
    {
        using var context = CreateContext();
        IFlightRepository repo = new FlightRepository(context);
        var flight = new Flight { Id = Guid.NewGuid(), FlightNumber = "LY400", Destination = "Berlin", Gate = "D1", DepartureTime = DateTime.UtcNow.AddHours(5) };
        await repo.AddFlightAsync(flight);
        var found = await repo.GetFlightByIdAsync(flight.Id);
        Assert.NotNull(found);
        Assert.Equal("LY400", found!.FlightNumber);
    }
} 