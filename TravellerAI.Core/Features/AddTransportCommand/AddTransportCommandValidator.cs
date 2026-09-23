using FluentValidation;
using static TravellerAI.Core.Constants.Validation;

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
            .WithMessage("Company cannot be empty")
            .MaximumLength(MaxTitleLength)
            .WithMessage($"Company cannot exceed {MaxTitleLength} characters");
        RuleFor(x => x.SeatClass)
            .IsInEnum()
            .WithMessage("Invalid seat class");
        RuleFor(x => x.SeatCount)
            .GreaterThanOrEqualTo(MinSeatCount)
            .WithMessage($"Seat count must be at least {MinSeatCount}");
        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(MinPrice)
            .WithMessage($"Price must be greater than or equal {MinPrice}");
    }
}
