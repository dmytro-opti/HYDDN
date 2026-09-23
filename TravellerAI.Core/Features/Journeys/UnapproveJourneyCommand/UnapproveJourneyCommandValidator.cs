using FluentValidation;

namespace TravellerAI.Core.Features.Journeys.UnapproveJourneyCommand;

public class UnapproveJourneyCommandValidator : AbstractValidator<UnapproveJourneyCommand>
{
    public UnapproveJourneyCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId cannot be empty");
        RuleFor(x => x.JourneyId).NotEmpty().WithMessage("JourneyId cannot be empty");
    }
}
