using FluentValidation;

namespace TravellerAI.Core.Features.BuildJourneyCommand;

public class BuildJourneyCommandValidator : AbstractValidator<BuildJourneyCommand>
{
    public BuildJourneyCommandValidator()
    {
        RuleFor(input => input.UserId)
            .NotEmpty().WithMessage("UserId cannot be null");
    }
}