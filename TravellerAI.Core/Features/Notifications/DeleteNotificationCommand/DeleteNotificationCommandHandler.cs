using MediatR;
using TravellerAI.Core.Interfaces;

namespace TravellerAI.Core.Features.Notifications.DeleteNotificationCommand;

public class DeleteNotificationCommandHandler : IRequestHandler<DeleteNotificationCommand, Unit>
{
    private readonly INotificationService _notificationService;

    public DeleteNotificationCommandHandler(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task<Unit> Handle(DeleteNotificationCommand command, CancellationToken cancellationToken)
    {
        await _notificationService.DeleteNotificationAsync(command.UserId, command.NotificationId);

        return Unit.Value;
    }
}
