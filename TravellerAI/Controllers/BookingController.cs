using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TravellerAI.Core.Features.SelectBookingCommand;
using TravellerAI.Core.Features.UpdateBookingCommand;
using TravellerAI.Domain.ViewModels.Requests;

namespace TravellerAI.WebApi.Controllers;

/// <summary>
/// Trip bookings.
/// </summary>
/// <remarks>
/// Exceptions are translated to HTTP responses by GlobalExceptionHandler.
/// </remarks>
[ApiController]
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
    /// Updates booking place, room, guests, dates and payment method.
    /// </summary>
    [HttpPut("{bookingId:guid}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<bool>> Update(Guid bookingId, [FromBody] UpdateBookingRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<UpdateBookingCommand>(request);
        command.BookingId = bookingId;

        return await _mediator.Send(command, cancellationToken);
    }

    /// <summary>
    /// Selects (freezes) the trip booking. Returns 422 when the booking is not valid anymore
    /// (closed, past dates or the place is already taken).
    /// </summary>
    [HttpPost("{bookingId:guid}/select")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<bool>> Select(Guid bookingId, [FromBody] SelectBookingRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<SelectBookingCommand>(request);
        command.BookingId = bookingId;

        var isSelected = await _mediator.Send(command, cancellationToken);
        if (!isSelected)
        {
            return Problem(
                title: "Booking is not valid",
                detail: $"Booking {bookingId} cannot be selected: it is closed, has past dates or the place is already taken",
                statusCode: StatusCodes.Status422UnprocessableEntity);
        }

        return isSelected;
    }
}
