using FluentValidation;

namespace TravellerAI.Core.Features.Notifications.GetNotificationsCommand;

public class GetNotificationsCommandValidator : AbstractValidator<GetNotificationsCommand>
{
    public GetNotificationsCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId cannot be empty");
    }
}
