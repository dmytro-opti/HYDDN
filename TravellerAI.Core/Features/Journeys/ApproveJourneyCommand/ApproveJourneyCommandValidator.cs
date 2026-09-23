using FluentValidation;

namespace TravellerAI.Core.Features.Journeys.ApproveJourneyCommand;

public class ApproveJourneyCommandValidator : AbstractValidator<ApproveJourneyCommand>
{
    public ApproveJourneyCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId cannot be empty");
        RuleFor(x => x.JourneyId).NotEmpty().WithMessage("JourneyId cannot be empty");
    }
}
