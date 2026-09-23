using MediatR;

namespace TravellerAI.Core.Features.Notifications.DeleteNotificationCommand;

public class DeleteNotificationCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public Guid NotificationId { get; set; }
}
