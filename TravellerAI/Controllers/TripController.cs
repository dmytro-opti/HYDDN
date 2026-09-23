using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravellerAI.Auth;
using TravellerAI.Core.Features.AddBookingCommand;
using TravellerAI.Core.Features.AddTransportCommand;
using TravellerAI.Core.Features.BuildTripCommand;
using TravellerAI.Core.Features.GetTripStatusCommand;
using TravellerAI.Core.Features.UpdateTripCommand;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.ViewModels;
using TravellerAI.Domain.ViewModels.Requests;

namespace TravellerAI.WebApi.Controllers;

/// <summary>
/// Trips, their transports and bookings.
/// </summary>
/// <remarks>
/// Exceptions are translated to HTTP responses by GlobalExceptionHandler.
/// </remarks>
[ApiController]
[Authorize]
[Route("api/trips")]
[Produces("application/json")]
public class TripController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public TripController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Returns trip status.
    /// </summary>
    [HttpGet("{tripId:guid}/status")]
    [ProducesResponseType(typeof(StatusViewModel<TripStatus>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StatusViewModel<TripStatus>>> GetStatus(Guid tripId, CancellationToken cancellationToken)
    {
        var status = await _mediator.Send(new GetTripStatusCommand { TripId = tripId, UserId = User.GetUserId() }, cancellationToken);

        return new StatusViewModel<TripStatus> { Id = tripId, Status = status };
    }

    /// <summary>
    /// Updates trip name, period, rating and (optionally) its booking.
    /// </summary>
    [HttpPut("{tripId:guid}")]
    [ProducesResponseType(typeof(TripViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<TripViewModel>> Update(Guid tripId, [FromBody] UpdateTripRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<UpdateTripCommand>(request);
        command.TripId = tripId;
        command.UserId = User.GetUserId();

        var trip = await _mediator.Send(command, cancellationToken);

        return _mapper.Map<TripViewModel>(trip);
    }

    /// <summary>
    /// Sets trip budget and period, applies journeys settings and transports, then recalculates totals.
    /// </summary>
    [HttpPost("{tripId:guid}/build")]
    [ProducesResponseType(typeof(TripViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<TripViewModel>> Build(Guid tripId, [FromBody] BuildTripRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<BuildTripCommand>(request);
        command.TripId = tripId;
        command.UserId = User.GetUserId();

        var trip = await _mediator.Send(command, cancellationToken);

        return _mapper.Map<TripViewModel>(trip);
    }

    /// <summary>
    /// Adds transport to the trip (optionally linked to a journey).
    /// </summary>
    [HttpPost("{tripId:guid}/transports")]
    [ProducesResponseType(typeof(TransportViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<TransportViewModel>> AddTransport(Guid tripId, [FromBody] AddTransportRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<AddTransportCommand>(request);
        command.TripId = tripId;
        command.UserId = User.GetUserId();

        var transport = await _mediator.Send(command, cancellationToken);

        return StatusCode(StatusCodes.Status201Created, _mapper.Map<TransportViewModel>(transport));
    }

    /// <summary>
    /// Adds a new booking to the trip. A previous not frozen booking is cancelled.
    /// </summary>
    [HttpPost("{tripId:guid}/bookings")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<bool>> AddBooking(Guid tripId, [FromBody] AddBookingRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<AddBookingCommand>(request);
        command.TripId = tripId;
        command.UserId = User.GetUserId();

        return await _mediator.Send(command, cancellationToken);
    }
}
