using FluentValidation;

namespace TravellerAI.Core.Features.CreateJourneyCommand;

public class CreateJourneyCommandValidator : AbstractValidator<CreateJourneyCommand>
{
    public CreateJourneyCommandValidator()
    {
        RuleFor(input => input.UserId)
            .NotNull().WithMessage("UserId cannot be null");
    }
}