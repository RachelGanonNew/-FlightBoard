using AutoMapper;
using FlightBoard.Domain.Entities;
using FlightBoard.Application.DTOs;

namespace FlightBoard.Application.Mapping;

public class FlightProfile : Profile
{
    public FlightProfile()
    {
        CreateMap<Flight, FlightWithStatusDto>()
            .ForMember(dest => dest.Status, opt => opt.Ignore()); // Status is set separately
        CreateMap<FlightWithStatusDto, Flight>();
    }
} 