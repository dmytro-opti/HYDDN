using FluentValidation;

namespace TravellerAI.Core.Features.Journeys.DeleteJourneyCommand;

public class DeleteJourneyCommandValidator : AbstractValidator<DeleteJourneyCommand>
{
    public DeleteJourneyCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId cannot be empty");
        RuleFor(x => x.JourneyId).NotEmpty().WithMessage("JourneyId cannot be empty");
    }
}
