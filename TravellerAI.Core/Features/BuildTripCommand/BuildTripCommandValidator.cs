using FluentValidation;
using static TravellerAI.Core.Constants.Validation;

namespace TravellerAI.Core.Features.BuildTripCommand;

public class BuildTripCommandValidator : AbstractValidator<BuildTripCommand>
{
    public BuildTripCommandValidator()
    {
        RuleFor(input => input.TripId)
            .NotEmpty().WithMessage("TripId cannot be empty");

        RuleFor(input => input.User!.Id)
            .NotEmpty().WithMessage("UserId cannot be empty")
            .When(input => input.User != null);

        RuleFor(input => input.Period)
            .NotNull().WithMessage("Period cannot be null");

        RuleFor(input => input.Period)
            .Must(period => period.Start < period.End)
            .When(input => input.Period != null)
            .WithMessage("Period start date must be before end date");

        RuleFor(input => input.Budget)
            .GreaterThanOrEqualTo(MinBudget).WithMessage($"Budget must be greater than or equal {MinBudget}");

        RuleForEach(input => input.Journeys)
            .Must(journey => journey.Id != Guid.Empty).WithMessage("Journey Id cannot be empty")
            .Must(journey => journey.Budget >= MinBudget).WithMessage($"Journey budget must be greater than or equal {MinBudget}")
            .Must(journey => journey.Period == null || journey.Period.Start < journey.Period.End)
            .WithMessage("Journey period start date must be before end date")
            .When(input => input.Journeys != null);
    }
}
