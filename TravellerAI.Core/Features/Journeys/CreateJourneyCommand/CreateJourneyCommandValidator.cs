using FluentValidation;
using static TravellerAI.Core.Constants;
using static TravellerAI.Core.Constants.Validation;

namespace TravellerAI.Core.Features.Journeys.CreateJourneyCommand;

public class CreateJourneyCommandValidator : AbstractValidator<CreateJourneyCommand>
{
    public CreateJourneyCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId cannot be empty");
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title cannot be empty").MaximumLength(MaxTitleLength).WithMessage($"Title cannot exceed {MaxTitleLength} characters");
        RuleFor(x => x.Description).MaximumLength(MaxTextLength).WithMessage($"Description cannot exceed {MaxTextLength} characters");
        RuleFor(x => x.Members).Must(m => m!.Count <= Journey.MaxMembers).When(x => x.Members != null).WithMessage($"Up to {Journey.MaxMembers} members");
        RuleForEach(x => x.Members).MaximumLength(MaxNameLength).WithMessage($"Member name cannot exceed {MaxNameLength} characters");
    }
}
