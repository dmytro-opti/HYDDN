using FluentValidation;

namespace TravellerAI.Core.Features.Journeys.SelectJourneyCountryCommand;

public class SelectJourneyCountryCommandValidator : AbstractValidator<SelectJourneyCountryCommand>
{
    public SelectJourneyCountryCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId cannot be empty");
        RuleFor(x => x.JourneyId).NotEmpty().WithMessage("JourneyId cannot be empty");
        RuleFor(x => x.CountryId).NotEmpty().WithMessage("CountryId cannot be empty");
    }
}
