using FluentValidation;
using static TravellerAI.Core.Constants;
using static TravellerAI.Core.Constants.Validation;

namespace TravellerAI.Core.Features.Journeys.AddJourneyTransportCommand;

public class AddJourneyTransportCommandValidator : AbstractValidator<AddJourneyTransportCommand>
{
    public AddJourneyTransportCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId cannot be empty");
        RuleFor(x => x.JourneyId).NotEmpty().WithMessage("JourneyId cannot be empty");
        RuleFor(x => x.Type).IsInEnum().WithMessage("Invalid transport type");
        RuleFor(x => x.SeatClass).IsInEnum().WithMessage("Invalid seat class");
        RuleFor(x => x.Company).NotEmpty().WithMessage("Company cannot be empty").MaximumLength(MaxTitleLength).WithMessage($"Company cannot exceed {MaxTitleLength} characters");
        RuleFor(x => x.SeatCount).GreaterThanOrEqualTo(MinSeatCount).WithMessage($"Seat count must be at least {MinSeatCount}");
        RuleFor(x => x.Price).GreaterThanOrEqualTo(MinPrice).WithMessage($"Price must be greater than or equal {MinPrice}");
        RuleFor(x => x.Period).Must(p => p!.Start < p.End).When(x => x.Period != null).WithMessage("Departure has to be before arrival");
    }
}
