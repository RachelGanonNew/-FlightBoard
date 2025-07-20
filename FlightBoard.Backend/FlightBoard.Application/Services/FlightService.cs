using FlightBoard.Application.DTOs;
using FlightBoard.Application.Interfaces;
using FlightBoard.Domain.Entities;
using FlightBoard.Domain.Enums;
using AutoMapper;

namespace FlightBoard.Application.Services;

public class FlightService : IFlightService
{
    private readonly IFlightRepository _repository;
    private readonly IFlightStatusService _statusService;
    private readonly IMapper _mapper;

    public FlightService(IFlightRepository repository, IFlightStatusService statusService, IMapper mapper)
    {
        _repository = repository;
        _statusService = statusService;
        _mapper = mapper;
    }

    public async Task<(bool IsSuccess, string? Error)> CreateFlightAsync(FlightCreateDto dto)
    {
        if (await _repository.FlightNumberExistsAsync(dto.FlightNumber))
            return (false, "Flight number already exists");

        var flight = new Flight
        {
            Id = Guid.NewGuid(),
            FlightNumber = dto.FlightNumber,
            Destination = dto.Destination,
            Gate = dto.Gate,
            DepartureTime = dto.DepartureTime
        };
        await _repository.AddFlightAsync(flight);
        return (true, null);
    }

    public async Task<bool> DeleteFlightAsync(Guid id)
    {
        var flight = await _repository.GetFlightByIdAsync(id);
        if (flight == null)
            return false;
        await _repository.DeleteFlightAsync(id);
        return true;
    }

    public async Task<List<FlightWithStatusDto>> GetAllFlightsAsync()
    {
        var now = DateTime.UtcNow;
        var flights = await _repository.GetAllFlightsAsync();
        var result = _mapper.Map<List<FlightWithStatusDto>>(flights);
        foreach (var dto in result)
        {
            var flight = flights.First(f => f.Id == dto.Id);
            dto.Status = _statusService.GetStatus(flight, now).ToString();
        }
        return result;
    }

    public async Task<List<FlightWithStatusDto>> SearchFlightsAsync(string? status, string? destination)
    {
        var now = DateTime.UtcNow;
        var flights = await _repository.GetAllFlightsAsync();
        var result = _mapper.Map<List<FlightWithStatusDto>>(flights);
        foreach (var dto in result)
        {
            var flight = flights.First(f => f.Id == dto.Id);
            dto.Status = _statusService.GetStatus(flight, now).ToString();
        }
        if (!string.IsNullOrWhiteSpace(destination))
            result = result.Where(f => f.Destination.Contains(destination, StringComparison.OrdinalIgnoreCase)).ToList();
        if (!string.IsNullOrEmpty(status))
            result = result.Where(f => f.Status.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();
        return result;
    }
} 