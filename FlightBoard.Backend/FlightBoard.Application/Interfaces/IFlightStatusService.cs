using FlightBoard.Domain.Entities;
using FlightBoard.Domain.Enums;

namespace FlightBoard.Application.Interfaces;

public interface IFlightStatusService
{
    FlightStatus GetStatus(Flight flight, DateTime currentTime);
} 