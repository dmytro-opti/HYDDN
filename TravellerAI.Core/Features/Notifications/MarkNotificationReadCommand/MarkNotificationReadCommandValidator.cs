using FluentValidation;

namespace TravellerAI.Core.Features.Notifications.MarkNotificationReadCommand;

public class MarkNotificationReadCommandValidator : AbstractValidator<MarkNotificationReadCommand>
{
    public MarkNotificationReadCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId cannot be empty");
        RuleFor(x => x.NotificationId).NotEmpty().WithMessage("NotificationId cannot be empty");
    }
}
