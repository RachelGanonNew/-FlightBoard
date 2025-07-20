using Xunit;
using Moq;
using FlightBoard.Domain.Entities;
using FlightBoard.Application.DTOs;
using FlightBoard.Application.Interfaces;
using FlightBoard.Application.Services;
using FlightBoard.Domain.Enums;
using AutoMapper;

public class FlightServiceTests
{
    private readonly Mock<IFlightRepository> _repoMock = new();
    private readonly Mock<IFlightStatusService> _statusMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly FlightService _service;

    public FlightServiceTests()
    {
        _service = new FlightService(_repoMock.Object, _statusMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Should_Not_Allow_Duplicate_FlightNumber()
    {
        _repoMock.Setup(r => r.FlightNumberExistsAsync("LY001", default)).ReturnsAsync(true);
        var dto = new FlightCreateDto { FlightNumber = "LY001", Destination = "Berlin", Gate = "C3", DepartureTime = DateTime.UtcNow.AddMinutes(50) };
        var result = await _service.CreateFlightAsync(dto);
        Assert.False(result.IsSuccess);
        Assert.Equal("Flight number already exists", result.Error);
    }

    [Fact]
    public async Task Should_Create_Flight_When_Valid()
    {
        _repoMock.Setup(r => r.FlightNumberExistsAsync(It.IsAny<string>(), default)).ReturnsAsync(false);
        _repoMock.Setup(r => r.AddFlightAsync(It.IsAny<Flight>(), default)).Returns(Task.CompletedTask);
        var dto = new FlightCreateDto { FlightNumber = "LY002", Destination = "Paris", Gate = "A1", DepartureTime = DateTime.UtcNow.AddMinutes(60) };
        var result = await _service.CreateFlightAsync(dto);
        Assert.True(result.IsSuccess);
        Assert.Null(result.Error);
    }

    [Fact]
    public async Task Should_Delete_Flight_When_Exists()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.GetFlightByIdAsync(id, default)).ReturnsAsync(new Flight { Id = id });
        _repoMock.Setup(r => r.DeleteFlightAsync(id, default)).Returns(Task.CompletedTask);
        var result = await _service.DeleteFlightAsync(id);
        Assert.True(result);
    }

    [Fact]
    public async Task Should_Not_Delete_When_Flight_Not_Found()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.GetFlightByIdAsync(id, default)).ReturnsAsync((Flight?)null);
        var result = await _service.DeleteFlightAsync(id);
        Assert.False(result);
    }

    [Fact]
    public async Task Should_Get_All_Flights_With_Status()
    {
        var now = DateTime.UtcNow;
        var flights = new List<Flight> { new Flight { Id = Guid.NewGuid(), FlightNumber = "LY003", Destination = "NYC", Gate = "B2", DepartureTime = now.AddMinutes(40) } };
        _repoMock.Setup(r => r.GetAllFlightsAsync(default)).ReturnsAsync(flights);
        _statusMock.Setup(s => s.GetStatus(It.IsAny<Flight>(), It.IsAny<DateTime>())).Returns(FlightStatus.Boarding);
        _mapperMock.Setup(m => m.Map<List<FlightWithStatusDto>>(It.IsAny<List<Flight>>())).Returns(new List<FlightWithStatusDto> { new FlightWithStatusDto { Id = flights[0].Id, FlightNumber = flights[0].FlightNumber, Destination = flights[0].Destination, DepartureTime = flights[0].DepartureTime, Gate = flights[0].Gate } });
        var result = await _service.GetAllFlightsAsync();
        Assert.Single(result);
        Assert.Equal("Boarding", result[0].Status);
    }

    [Fact]
    public async Task Should_Search_Flights_By_Status_And_Destination()
    {
        var now = DateTime.UtcNow;
        var flights = new List<Flight> {
            new Flight { Id = Guid.NewGuid(), FlightNumber = "LY004", Destination = "London", Gate = "C1", DepartureTime = now.AddMinutes(90) },
            new Flight { Id = Guid.NewGuid(), FlightNumber = "LY005", Destination = "London", Gate = "C2", DepartureTime = now.AddMinutes(10) }
        };
        _repoMock.Setup(r => r.GetAllFlightsAsync(default)).ReturnsAsync(flights);
        _statusMock.Setup(s => s.GetStatus(It.IsAny<Flight>(), It.IsAny<DateTime>())).Returns((Flight f, DateTime t) => f.DepartureTime > now.AddMinutes(30) ? FlightStatus.Scheduled : FlightStatus.Boarding);
        _mapperMock.Setup(m => m.Map<List<FlightWithStatusDto>>(It.IsAny<List<Flight>>())).Returns(flights.Select(f => new FlightWithStatusDto { Id = f.Id, FlightNumber = f.FlightNumber, Destination = f.Destination, DepartureTime = f.DepartureTime, Gate = f.Gate }).ToList());
        var result = await _service.SearchFlightsAsync("Boarding", "London");
        Assert.Single(result);
        Assert.Equal("Boarding", result[0].Status);
        Assert.Equal("London", result[0].Destination);
    }
}
