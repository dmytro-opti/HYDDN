using FluentValidation;
using static TravellerAI.Core.Constants.Validation;

namespace TravellerAI.Core.Features.BuildJourneyCommand;

public class BuildJourneyCommandValidator : AbstractValidator<BuildJourneyCommand>
{
    public BuildJourneyCommandValidator()
    {
        RuleFor(input => input.UserId)
            .NotEmpty().WithMessage("UserId cannot be empty");

        RuleFor(input => input.Title)
            .NotEmpty().WithMessage("Title cannot be empty")
            .MaximumLength(MaxTitleLength).WithMessage($"Title cannot exceed {MaxTitleLength} characters");

        RuleFor(input => input.Period)
            .Must(period => period.Start < period.End)
            .When(input => input.Period != null)
            .WithMessage("Period start date must be before end date");
    }
}
