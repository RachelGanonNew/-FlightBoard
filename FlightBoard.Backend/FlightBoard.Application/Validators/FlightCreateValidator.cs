using FluentValidation;
using FlightBoard.Application.DTOs;

namespace FlightBoard.Application.Validators;

public class FlightCreateValidator : AbstractValidator<FlightCreateDto>
{
    public FlightCreateValidator()
    {
        RuleFor(f => f.FlightNumber)
            .NotEmpty().WithMessage("Flight number is required.");

        RuleFor(f => f.Destination)
            .NotEmpty().WithMessage("Destination is required.");

        RuleFor(f => f.Gate)
            .NotEmpty().WithMessage("Gate is required.");

        RuleFor(f => f.DepartureTime)
            .Must(d => d > DateTime.UtcNow)
            .WithMessage("Departure time must be in the future.");
    }
}
