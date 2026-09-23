using FluentValidation;

namespace TravellerAI.Core.Features.Journeys.ClearDayTripCommand;

public class ClearDayTripCommandValidator : AbstractValidator<ClearDayTripCommand>
{
    public ClearDayTripCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId cannot be empty");
        RuleFor(x => x.JourneyId).NotEmpty().WithMessage("JourneyId cannot be empty");
    }
}
