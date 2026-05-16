using FluentValidation;

namespace TravellerAI.Core.Features.BuildTripCommand;

public class BuildTripCommandValidator : AbstractValidator<BuildTripCommand>
{
    public BuildTripCommandValidator()
    {
        RuleFor(input => input.User.Id)
            .NotNull().WithMessage("UserId cannot be null");
    }
} 