namespace FlightBoard.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using FlightBoard.Application.DTOs;
using FluentValidation;
using FlightBoard.API.Hubs;
using Microsoft.AspNetCore.SignalR;
using FlightBoard.Application.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class FlightsController : ControllerBase
{
    private readonly IFlightService _flightService;
    private readonly IValidator<FlightCreateDto> _validator;
    private readonly IHubContext<FlightHub> _hub;

    public FlightsController(IFlightService flightService, IValidator<FlightCreateDto> validator, IHubContext<FlightHub> hub)
    {
        _flightService = flightService;
        _validator = validator;
        _hub = hub;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _flightService.GetAllFlightsAsync();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] FlightCreateDto dto)
    {
        var validation = await _validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => e.ErrorMessage));

        var (isSuccess, error) = await _flightService.CreateFlightAsync(dto);
        if (!isSuccess)
            return BadRequest(error);

        // Optionally, fetch the created flight to broadcast (not shown here for brevity)
        await _hub.Clients.All.SendAsync("FlightAdded", dto);
        return CreatedAtAction(nameof(GetAll), null);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _flightService.DeleteFlightAsync(id);
        if (!success)
            return NotFound();
        await _hub.Clients.All.SendAsync("FlightDeleted", id);
        return NoContent();
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string? status, [FromQuery] string? destination)
    {
        var result = await _flightService.SearchFlightsAsync(status, destination);
        return Ok(result);
    }
}
