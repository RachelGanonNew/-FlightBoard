using Xunit;
using FlightBoard.Application.DTOs;
using FlightBoard.Application.Validators;

public class FlightCreateValidatorTests
{
    private readonly FlightCreateValidator _validator = new();

    [Fact]
    public void Should_Pass_Validation_For_Valid_Data()
    {
        var dto = new FlightCreateDto
        {
            FlightNumber = "LY123",
            Destination = "Rome",
            Gate = "A1",
            DepartureTime = DateTime.UtcNow.AddHours(1)
        };

        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Should_Fail_Validation_For_Missing_Fields()
    {
        var dto = new FlightCreateDto(); // all empty

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "FlightNumber");
        Assert.Contains(result.Errors, e => e.PropertyName == "Destination");
        Assert.Contains(result.Errors, e => e.PropertyName == "Gate");
    }

    [Fact]
    public void Should_Fail_When_DepartureTime_Is_Past()
    {
        var dto = new FlightCreateDto
        {
            FlightNumber = "LY001",
            Destination = "Paris",
            Gate = "B2",
            DepartureTime = DateTime.UtcNow.AddMinutes(-10)
        };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "DepartureTime");
    }
}
