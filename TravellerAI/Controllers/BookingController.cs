using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravellerAI.Auth;
using TravellerAI.Core.Features.Bookings.GetBookingCommand;
using TravellerAI.Core.Features.Bookings.PayBookingCommand;
using TravellerAI.Domain.ViewModels;
using TravellerAI.Domain.ViewModels.Requests;

namespace TravellerAI.WebApi.Controllers;

/// <summary>
/// Hotel bookings. They are created and changed through the journey hotels endpoints.
/// </summary>
[ApiController]
[Authorize]
[Route("api/bookings")]
[Produces("application/json")]
public class BookingController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public BookingController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Booking of the current user.
    /// </summary>
    [HttpGet("{bookingId:guid}")]
    [ProducesResponseType(typeof(BookingViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BookingViewModel>> Get(Guid bookingId, CancellationToken cancellationToken)
    {
        var booking = await _mediator.Send(new GetBookingCommand { BookingId = bookingId, UserId = User.GetUserId() }, cancellationToken);

        return _mapper.Map<BookingViewModel>(booking);
    }

    /// <summary>
    /// Pays the booking: it becomes confirmed and its dates cannot be changed anymore.
    /// </summary>
    [HttpPost("{bookingId:guid}/pay")]
    [ProducesResponseType(typeof(BookingViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<BookingViewModel>> Pay(Guid bookingId, [FromBody] PayBookingRequest request, CancellationToken cancellationToken)
    {
        var booking = await _mediator.Send(new PayBookingCommand
        {
            BookingId = bookingId,
            UserId = User.GetUserId(),
            PaymentMethod = request.PaymentMethod
        }, cancellationToken);

        return _mapper.Map<BookingViewModel>(booking);
    }
}
