using FlightBoard.Application.DTOs;

namespace FlightBoard.Application.Interfaces;

public interface IFlightService
{
    Task<(bool IsSuccess, string? Error)> CreateFlightAsync(FlightCreateDto dto);
    Task<bool> DeleteFlightAsync(Guid id);
    Task<List<FlightWithStatusDto>> GetAllFlightsAsync();
    Task<List<FlightWithStatusDto>> SearchFlightsAsync(string? status, string? destination);
}