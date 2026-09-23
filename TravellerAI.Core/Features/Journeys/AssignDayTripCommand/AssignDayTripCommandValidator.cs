using FluentValidation;

namespace TravellerAI.Core.Features.Journeys.AssignDayTripCommand;

public class AssignDayTripCommandValidator : AbstractValidator<AssignDayTripCommand>
{
    public AssignDayTripCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId cannot be empty");
        RuleFor(x => x.JourneyId).NotEmpty().WithMessage("JourneyId cannot be empty");
        RuleFor(x => x.TripId).NotEmpty().WithMessage("TripId cannot be empty");
    }
}
