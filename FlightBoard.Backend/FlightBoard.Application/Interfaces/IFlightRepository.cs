namespace FlightBoard.Application.Interfaces;

using FlightBoard.Domain.Entities;
using FlightBoard.Domain.Enums;

public interface IFlightRepository
{
    Task<bool> FlightNumberExistsAsync(string flightNumber, CancellationToken cancellationToken = default);
    Task AddFlightAsync(Flight flight, CancellationToken cancellationToken = default);
    Task DeleteFlightAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Flight?> GetFlightByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Flight>> GetAllFlightsAsync(CancellationToken cancellationToken = default);
    Task<List<Flight>> SearchFlightsAsync(FlightStatus? status, string? destination, CancellationToken cancellationToken = default);
} 