using MediatR;
using TravellerAI.Core.Interfaces;

namespace TravellerAI.Core.Features.Notifications.MarkNotificationReadCommand;

public class MarkNotificationReadCommandHandler : IRequestHandler<MarkNotificationReadCommand, Unit>
{
    private readonly INotificationService _notificationService;

    public MarkNotificationReadCommandHandler(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task<Unit> Handle(MarkNotificationReadCommand command, CancellationToken cancellationToken)
    {
        await _notificationService.MarkAsReadAsync(command.UserId, command.NotificationId);

        return Unit.Value;
    }
}
