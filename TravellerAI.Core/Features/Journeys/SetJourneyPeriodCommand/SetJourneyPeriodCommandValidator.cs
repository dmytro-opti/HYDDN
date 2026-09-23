using FluentValidation;
using static TravellerAI.Core.Constants;
using static TravellerAI.Core.Constants.Validation;

namespace TravellerAI.Core.Features.Journeys.SetJourneyPeriodCommand;

public class SetJourneyPeriodCommandValidator : AbstractValidator<SetJourneyPeriodCommand>
{
    public SetJourneyPeriodCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId cannot be empty");
        RuleFor(x => x.JourneyId).NotEmpty().WithMessage("JourneyId cannot be empty");
        RuleFor(x => x.Start.Date).GreaterThanOrEqualTo(_ => DateTime.UtcNow.Date).WithMessage("Journey cannot start in the past");
        RuleFor(x => x.End.Date).GreaterThan(x => x.Start.Date).WithMessage("Journey has to end after it starts (at least one night)");
        RuleFor(x => x).Must(x => (x.End.Date - x.Start.Date).Days + 1 <= Journey.MaxJourneyDays).WithMessage($"Journey cannot be longer than {Journey.MaxJourneyDays} days");
    }
}
