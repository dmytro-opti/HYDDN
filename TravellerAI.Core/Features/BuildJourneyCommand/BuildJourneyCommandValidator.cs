using FluentValidation;

namespace TravellerAI.Core.Features.BuildJourneyCommand;

public class BuildJourneyCommandValidator : AbstractValidator<BuildJourneyCommand>
{
    public BuildJourneyCommandValidator()
    {
        RuleFor(input => input.UserId)
            .NotNull().WithMessage("UserId cannot be null");
    }
}