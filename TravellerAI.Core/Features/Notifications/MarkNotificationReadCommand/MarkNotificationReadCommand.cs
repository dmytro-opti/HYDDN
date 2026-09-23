using MediatR;

namespace TravellerAI.Core.Features.Notifications.MarkNotificationReadCommand;

public class MarkNotificationReadCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public Guid NotificationId { get; set; }
}
