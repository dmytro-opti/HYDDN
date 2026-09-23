using FluentValidation;

namespace TravellerAI.Core.Features.Journeys.GetJourneyProgressCommand;

public class GetJourneyProgressCommandValidator : AbstractValidator<GetJourneyProgressCommand>
{
    public GetJourneyProgressCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId cannot be empty");
        RuleFor(x => x.JourneyId).NotEmpty().WithMessage("JourneyId cannot be empty");
    }
}
