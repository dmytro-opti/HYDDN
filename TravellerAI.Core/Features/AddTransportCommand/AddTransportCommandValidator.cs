using FluentValidation;

namespace TravellerAI.Core.Features.AddTransportCommand;

public class AddTransportCommandValidator : AbstractValidator<AddTransportCommand>
{
    public AddTransportCommandValidator()
    {
        RuleFor(x => x.TripId)
            .NotEmpty()
            .WithMessage("TripId cannot be empty");
        RuleFor(x => x.JourneyId)
            .NotEmpty()
            .WithMessage("JourneyId cannot be empty")
            .When(x => x.JourneyId.HasValue);
        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Invalid transport type");
        RuleFor(x => x.Company)
            .NotEmpty()
            .WithMessage("Company cannot be empty");
        RuleFor(x => x.SeatClass)
            .IsInEnum()
            .WithMessage("Invalid seat class");
        RuleFor(x => x.SeatCount)
            .GreaterThan(0)
            .WithMessage("Seat count must be greater than 0");
    }
}