using FluentValidation;

namespace TravellerAI.Core.Features.BuildJourneyCommand;

public class BuildTripCommandValidator : AbstractValidator<BuildTripCommand>
{
    public BuildTripCommandValidator()
    {
        RuleFor(input => input.User.Id)
            .NotNull().WithMessage("UserId cannot be null");
    }
}