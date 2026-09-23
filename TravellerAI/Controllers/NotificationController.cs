using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravellerAI.Auth;
using TravellerAI.Core.Features.Notifications.DeleteNotificationCommand;
using TravellerAI.Core.Features.Notifications.GetNotificationsCommand;
using TravellerAI.Core.Features.Notifications.MarkNotificationReadCommand;
using TravellerAI.Domain.ViewModels;

namespace TravellerAI.WebApi.Controllers;

/// <summary>
/// Notifications of the current user.
/// </summary>
[ApiController]
[Authorize]
[Route("api/notifications")]
[Produces("application/json")]
public class NotificationController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public NotificationController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Notifications, newest first.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<NotificationViewModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<NotificationViewModel>>> Get([FromQuery] bool unreadOnly, CancellationToken cancellationToken)
    {
        var notifications = await _mediator.Send(new GetNotificationsCommand { UserId = User.GetUserId(), UnreadOnly = unreadOnly },
            cancellationToken);

        return _mapper.Map<List<NotificationViewModel>>(notifications);
    }

    /// <summary>
    /// Marks the notification as read.
    /// </summary>
    [HttpPut("{notificationId:guid}/read")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkAsRead(Guid notificationId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new MarkNotificationReadCommand { NotificationId = notificationId, UserId = User.GetUserId() }, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Deletes the notification.
    /// </summary>
    [HttpDelete("{notificationId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete(Guid notificationId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteNotificationCommand { NotificationId = notificationId, UserId = User.GetUserId() }, cancellationToken);

        return NoContent();
    }
}
