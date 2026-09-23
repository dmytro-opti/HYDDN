using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Notifications.GetNotificationsCommand;

public class GetNotificationsCommandHandler : IRequestHandler<GetNotificationsCommand, List<NotificationModel>>
{
    private readonly INotificationService _notificationService;

    public GetNotificationsCommandHandler(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task<List<NotificationModel>> Handle(GetNotificationsCommand command, CancellationToken cancellationToken)
    {
        return (await _notificationService.GetNotificationsAsync(command.UserId, command.UnreadOnly)).ToList();
    }
}
