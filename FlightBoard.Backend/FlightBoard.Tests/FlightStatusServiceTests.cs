using Xunit;
using FlightBoard.Application.Services;
using FlightBoard.Domain.Entities;
using FlightBoard.Domain.Enums;

public class FlightStatusServiceTests
{
    private readonly FlightStatusService _service = new();

    [Theory]
    [InlineData(40, FlightStatus.Scheduled)]   // > 30 min before
    [InlineData(20, FlightStatus.Boarding)]    // 0 < min <= 30
    [InlineData(0, FlightStatus.Departed)]     // 0 min (departure)
    [InlineData(-30, FlightStatus.Departed)]   // -60 < min <= 0
    [InlineData(-70, FlightStatus.Landed)]     // <= -60 min
    public void Should_Return_Correct_Status(double minutesOffset, FlightStatus expected)
    {
        // Arrange
        var now = DateTime.UtcNow;
        var flight = new Flight
        {
            DepartureTime = now.AddMinutes(minutesOffset)
        };

        // Act
        var result = _service.GetStatus(flight, now);

        // Assert
        Assert.Equal(expected, result);
    }
}
