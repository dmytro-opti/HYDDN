using FluentValidation;

namespace TravellerAI.Core.Features.BuildJourneyCommand;

public class AddTransportValidator : AbstractValidator<BuildJourneyCommand>
{
    public AddTransportValidator()
    {
        RuleFor(input => input.UserId)
            .NotEmpty().WithMessage("UserId cannot be null");
        RuleFor(input => input.Title)
            .NotEmpty().WithMessage("Title cannot be null").MaximumLength(100).WithMessage("Title cannot exceed 100 characters");
        RuleFor(input => input.Description)
            .NotEmpty().WithMessage("Description cannot be null").MaximumLength(500).WithMessage("Description cannot exceed 500 characters");
        RuleFor(input => input.MemberIds)
            .NotEmpty().WithMessage("Must have at least one member");
        RuleForEach(input => input.MemberIds)
            .NotEmpty().WithMessage("Member ID cannot be empty");
    }
}