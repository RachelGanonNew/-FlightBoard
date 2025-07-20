using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FlightBoard.Application.DTOs;
using FlightBoard.API.Controllers;
using FlightBoard.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.SignalR;
using FlightBoard.API.Hubs;

public class FlightsControllerTests
{
    private readonly Mock<IFlightService> _serviceMock = new();
    private readonly Mock<IValidator<FlightCreateDto>> _validatorMock = new();
    private readonly Mock<IHubContext<FlightHub>> _hubMock = new();
    private readonly FlightsController _controller;

    public FlightsControllerTests()
    {
        var clientProxyMock = new Mock<IClientProxy>();
        _hubMock.Setup(h => h.Clients.All).Returns(clientProxyMock.Object);
        _controller = new FlightsController(_serviceMock.Object, _validatorMock.Object, _hubMock.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOkWithFlights()
    {
        _serviceMock.Setup(s => s.GetAllFlightsAsync()).ReturnsAsync(new List<FlightWithStatusDto> { new FlightWithStatusDto { FlightNumber = "LY001" } });
        var result = await _controller.GetAll();
        var ok = Assert.IsType<OkObjectResult>(result);
        var flights = Assert.IsAssignableFrom<IEnumerable<FlightWithStatusDto>>(ok.Value);
        Assert.Single(flights);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenInvalid()
    {
        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<FlightCreateDto>(), default)).ReturnsAsync(new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("FlightNumber", "Required") }));
        var result = await _controller.Create(new FlightCreateDto());
        var bad = Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenServiceFails()
    {
        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<FlightCreateDto>(), default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _serviceMock.Setup(s => s.CreateFlightAsync(It.IsAny<FlightCreateDto>())).ReturnsAsync((false, "error"));
        var result = await _controller.Create(new FlightCreateDto());
        var bad = Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Create_ReturnsCreated_WhenSuccess()
    {
        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<FlightCreateDto>(), default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _serviceMock.Setup(s => s.CreateFlightAsync(It.IsAny<FlightCreateDto>())).ReturnsAsync((true, null));
        // Do not setup SendAsync on SignalR, just run the controller method
        var result = await _controller.Create(new FlightCreateDto());
        Assert.IsType<CreatedAtActionResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenNotExists()
    {
        _serviceMock.Setup(s => s.DeleteFlightAsync(It.IsAny<Guid>())).ReturnsAsync(false);
        var result = await _controller.Delete(Guid.NewGuid());
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenSuccess()
    {
        _serviceMock.Setup(s => s.DeleteFlightAsync(It.IsAny<Guid>())).ReturnsAsync(true);
        // Do not setup SendAsync on SignalR, just run the controller method
        var result = await _controller.Delete(Guid.NewGuid());
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Search_ReturnsOkWithResults()
    {
        _serviceMock.Setup(s => s.SearchFlightsAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new List<FlightWithStatusDto> { new FlightWithStatusDto { FlightNumber = "LY002" } });
        var result = await _controller.Search("Scheduled", "Rome");
        var ok = Assert.IsType<OkObjectResult>(result);
        var flights = Assert.IsAssignableFrom<IEnumerable<FlightWithStatusDto>>(ok.Value);
        Assert.Single(flights);
    }
} 