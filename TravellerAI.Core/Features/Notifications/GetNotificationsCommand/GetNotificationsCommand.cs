using MediatR;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Notifications.GetNotificationsCommand;

public class GetNotificationsCommand : IRequest<List<NotificationModel>>
{
    public Guid UserId { get; set; }
    public bool UnreadOnly { get; set; }
}
