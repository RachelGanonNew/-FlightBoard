using FlightBoard.Application.Interfaces;
using FlightBoard.Domain.Entities;
using FlightBoard.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FlightBoard.Infrastructure.Persistence;

public class FlightRepository : IFlightRepository
{
    private readonly FlightDbContext _context;

    public FlightRepository(FlightDbContext context)
    {
        _context = context;
    }

    public async Task<bool> FlightNumberExistsAsync(string flightNumber, CancellationToken cancellationToken = default)
    {
        return await _context.Flights.AnyAsync(f => f.FlightNumber == flightNumber, cancellationToken);
    }

    public async Task AddFlightAsync(Flight flight, CancellationToken cancellationToken = default)
    {
        _context.Flights.Add(flight);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteFlightAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var flight = await _context.Flights.FindAsync(new object[] { id }, cancellationToken);
        if (flight != null)
        {
            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<Flight?> GetFlightByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Flights.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<List<Flight>> GetAllFlightsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Flights.ToListAsync(cancellationToken);
    }

    public async Task<List<Flight>> SearchFlightsAsync(FlightStatus? status, string? destination, CancellationToken cancellationToken = default)
    {
        var query = _context.Flights.AsQueryable();

        if (!string.IsNullOrWhiteSpace(destination))
            query = query.Where(f => f.Destination == destination);

        // Status filtering will be handled at the application/service layer since status is calculated, not stored.
        // This method returns all flights matching the destination, and the caller will filter by status after calculation.

        return await query.ToListAsync(cancellationToken);
    }
} 