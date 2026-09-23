using FluentValidation;
using static TravellerAI.Core.Constants;
using static TravellerAI.Core.Constants.Validation;

namespace TravellerAI.Core.Features.Journeys.SearchJourneyHotelsCommand;

public class SearchJourneyHotelsCommandValidator : AbstractValidator<SearchJourneyHotelsCommand>
{
    public SearchJourneyHotelsCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId cannot be empty");
        RuleFor(x => x.JourneyId).NotEmpty().WithMessage("JourneyId cannot be empty");
        RuleFor(x => x.Guests).GreaterThanOrEqualTo(MinAdults).WithMessage($"Guests must be at least {MinAdults}");
        RuleFor(x => x.City).MaximumLength(MaxLocationNameLength).WithMessage($"City cannot exceed {MaxLocationNameLength} characters");
    }
}
