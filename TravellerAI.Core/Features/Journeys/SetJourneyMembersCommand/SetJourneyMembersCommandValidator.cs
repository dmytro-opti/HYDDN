using FluentValidation;
using static TravellerAI.Core.Constants;
using static TravellerAI.Core.Constants.Validation;

namespace TravellerAI.Core.Features.Journeys.SetJourneyMembersCommand;

public class SetJourneyMembersCommandValidator : AbstractValidator<SetJourneyMembersCommand>
{
    public SetJourneyMembersCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId cannot be empty");
        RuleFor(x => x.JourneyId).NotEmpty().WithMessage("JourneyId cannot be empty");
        RuleFor(x => x.Members).NotNull().Must(m => m.Count <= Journey.MaxMembers).WithMessage($"Up to {Journey.MaxMembers} members");
        RuleForEach(x => x.Members).MaximumLength(MaxNameLength).WithMessage($"Member name cannot exceed {MaxNameLength} characters");
    }
}
