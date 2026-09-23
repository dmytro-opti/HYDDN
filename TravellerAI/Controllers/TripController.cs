using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravellerAI.Auth;
using TravellerAI.Core.Features.Trips.CreateTripCommand;
using TravellerAI.Core.Features.Trips.DeleteTripCommand;
using TravellerAI.Core.Features.Trips.GetTripCommand;
using TravellerAI.Core.Features.Trips.SearchTripsCommand;
using TravellerAI.Core.Features.Trips.UpdateTripCommand;
using TravellerAI.Domain.ViewModels;
using TravellerAI.Domain.ViewModels.Requests;

namespace TravellerAI.WebApi.Controllers;

/// <summary>
/// Trip routes: one day, ordered locations and activities in one city.
/// </summary>
/// <remarks>
/// Rules: 2-12 stops, the same country and city, limited distance between stops and per day.
/// A trip used on a journey day starts at the hotel of the previous night and finishes at the hotel of the coming night.
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
    /// Public and own trips of the country, optionally from / to a location (e.g. a hotel).
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<TripViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<TripViewModel>>> Search([FromQuery] TripSearchRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<SearchTripsCommand>(request);
        command.UserId = User.GetUserId();

        return _mapper.Map<List<TripViewModel>>(await _mediator.Send(command, cancellationToken));
    }

    /// <summary>
    /// Trip with its stops.
    /// </summary>
    [HttpGet("{tripId:guid}")]
    [ProducesResponseType(typeof(TripViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TripViewModel>> Get(Guid tripId, CancellationToken cancellationToken)
    {
        var trip = await _mediator.Send(new GetTripCommand { TripId = tripId, UserId = User.GetUserId() }, cancellationToken);

        return _mapper.Map<TripViewModel>(trip);
    }

    /// <summary>
    /// Creates a trip; optimize=true reorders the stops between the first and the last one by the shortest route.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(TripViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TripViewModel>> Create([FromBody] TripRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateTripCommand>(request);
        command.UserId = User.GetUserId();

        var trip = _mapper.Map<TripViewModel>(await _mediator.Send(command, cancellationToken));

        return CreatedAtAction(nameof(Get), new { tripId = trip.Id }, trip);
    }

    /// <summary>
    /// Changes the trip (author only; not possible when other travellers or approved journeys use it).
    /// </summary>
    [HttpPut("{tripId:guid}")]
    [ProducesResponseType(typeof(TripViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<TripViewModel>> Update(Guid tripId, [FromBody] TripRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<UpdateTripCommand>(request);
        command.TripId = tripId;
        command.UserId = User.GetUserId();

        return _mapper.Map<TripViewModel>(await _mediator.Send(command, cancellationToken));
    }

    /// <summary>
    /// Deletes the trip (author only; not possible when it is scheduled in journeys).
    /// </summary>
    [HttpDelete("{tripId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(Guid tripId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteTripCommand { TripId = tripId, UserId = User.GetUserId() }, cancellationToken);

        return NoContent();
    }
}
