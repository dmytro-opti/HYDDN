using FluentValidation;

namespace TravellerAI.Core.Features.Notifications.DeleteNotificationCommand;

public class DeleteNotificationCommandValidator : AbstractValidator<DeleteNotificationCommand>
{
    public DeleteNotificationCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId cannot be empty");
        RuleFor(x => x.NotificationId).NotEmpty().WithMessage("NotificationId cannot be empty");
    }
}
