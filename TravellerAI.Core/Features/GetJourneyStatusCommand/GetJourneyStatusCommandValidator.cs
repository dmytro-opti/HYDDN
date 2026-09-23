using FluentValidation;

namespace TravellerAI.Core.Features.GetJourneyStatusCommand;

public class GetJourneyStatusCommandValidator : AbstractValidator<GetJourneyStatusCommand>
{
    public GetJourneyStatusCommandValidator()
    {
        RuleFor(x => x.JourneyId)
            .NotEmpty()
            .WithMessage("JourneyId cannot be empty");
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId cannot be empty");
    }
}
