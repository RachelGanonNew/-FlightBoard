using FlightBoard.Domain.Entities;
using FlightBoard.Domain.Enums;
using FlightBoard.Application.Interfaces;

namespace FlightBoard.Application.Services;

public class FlightStatusService : IFlightStatusService
{
    public FlightStatus GetStatus(Flight flight, DateTime currentTime)
    {
        var timeDiff = (flight.DepartureTime - currentTime).TotalMinutes;

        if (timeDiff > 30)
            return FlightStatus.Scheduled;
        else if (timeDiff <= 30 && timeDiff > 0)
            return FlightStatus.Boarding;
        else if (timeDiff <= 0 && timeDiff > -60)
            return FlightStatus.Departed;
        else
            return FlightStatus.Landed;
    }
}
