using FluentValidation;
using static TravellerAI.Core.Constants;
using static TravellerAI.Core.Constants.Validation;

namespace TravellerAI.Core.Features.Journeys.SetJourneyBudgetCommand;

public class SetJourneyBudgetCommandValidator : AbstractValidator<SetJourneyBudgetCommand>
{
    public SetJourneyBudgetCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId cannot be empty");
        RuleFor(x => x.JourneyId).NotEmpty().WithMessage("JourneyId cannot be empty");
        RuleFor(x => x.Budget).GreaterThanOrEqualTo(MinBudget).WithMessage($"Budget must be greater than or equal {MinBudget}");
    }
}
