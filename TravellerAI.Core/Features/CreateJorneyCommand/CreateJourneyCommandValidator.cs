using FluentValidation;

namespace TravellerAI.Core.Features.CreateJourneyCommand;

public class CreateJourneyCommandValidator 
    : AbstractValidator<CreateJourneyCommand>
{
    public CreateJourneyCommandValidator()
    {
        RuleFor(x => x.Journey)
            .NotNull()
            .WithMessage("Journey is required.");
    }
}