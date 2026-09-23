using FluentValidation;

namespace TravellerAI.Core.Features.Journeys.GetJourneyCommand;

public class GetJourneyCommandValidator : AbstractValidator<GetJourneyCommand>
{
    public GetJourneyCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId cannot be empty");
        RuleFor(x => x.JourneyId).NotEmpty().WithMessage("JourneyId cannot be empty");
    }
}
