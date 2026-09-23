using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravellerAI.Auth;
using TravellerAI.Core.Features.GetJourneyStatusCommand;
using TravellerAI.Core.Features.Journeys.AddJourneyHotelCommand;
using TravellerAI.Core.Features.Journeys.AddJourneyTransportCommand;
using TravellerAI.Core.Features.Journeys.ApproveJourneyCommand;
using TravellerAI.Core.Features.Journeys.AssignDayTripCommand;
using TravellerAI.Core.Features.Journeys.ClearDayTripCommand;
using TravellerAI.Core.Features.Journeys.CreateJourneyCommand;
using TravellerAI.Core.Features.Journeys.DeleteJourneyCommand;
using TravellerAI.Core.Features.Journeys.GetDayTripsCommand;
using TravellerAI.Core.Features.Journeys.GetJourneyCommand;
using TravellerAI.Core.Features.Journeys.GetJourneyProgressCommand;
using TravellerAI.Core.Features.Journeys.GetMyJourneysCommand;
using TravellerAI.Core.Features.Journeys.RemoveJourneyHotelCommand;
using TravellerAI.Core.Features.Journeys.SearchJourneyHotelsCommand;
using TravellerAI.Core.Features.Journeys.SelectJourneyCountryCommand;
using TravellerAI.Core.Features.Journeys.SetJourneyBudgetCommand;
using TravellerAI.Core.Features.Journeys.SetJourneyMembersCommand;
using TravellerAI.Core.Features.Journeys.SetJourneyPeriodCommand;
using TravellerAI.Core.Features.Journeys.UnapproveJourneyCommand;
using TravellerAI.Core.Features.Journeys.UpdateJourneyHotelCommand;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;
using TravellerAI.Domain.ViewModels;
using TravellerAI.Domain.ViewModels.Requests;

namespace TravellerAI.WebApi.Controllers;

/// <summary>
/// Journey setup wizard: details -> period -> country -> hotels -> day trips -> approve.
/// </summary>
/// <remarks>
/// Drafts (Approved = false) can be continued any time; GET progress shows the next step.
/// Approved journeys are read-only: edits return 409 until the journey is unapproved.
/// </remarks>
[ApiController]
[Authorize]
[Route("api/journeys")]
[Produces("application/json")]
public class JourneyController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public JourneyController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Journeys of the current user; approved=false returns drafts to continue.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<JourneySummaryViewModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<JourneySummaryViewModel>>> GetMine([FromQuery] bool? approved, CancellationToken cancellationToken)
    {
        var journeys = await _mediator.Send(new GetMyJourneysCommand { UserId = User.GetUserId(), Approved = approved }, cancellationToken);

        return _mapper.Map<List<JourneySummaryViewModel>>(journeys);
    }

    /// <summary>
    /// Step 1: creates a draft journey. Returns the journey id.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateJourneyRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateJourneyCommand>(request);
        command.UserId = User.GetUserId();

        var journeyId = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(Get), new { journeyId }, journeyId);
    }

    /// <summary>
    /// Journey with days, hotel stays, transports and budget.
    /// </summary>
    [HttpGet("{journeyId:guid}")]
    [ProducesResponseType(typeof(JourneyViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public Task<ActionResult<JourneyViewModel>> Get(Guid journeyId, CancellationToken cancellationToken) =>
        Journey(new GetJourneyCommand { JourneyId = journeyId, UserId = User.GetUserId() }, cancellationToken);

    /// <summary>
    /// Deletes a not approved journey and cancels its not paid hotel bookings.
    /// </summary>
    [HttpDelete("{journeyId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(Guid journeyId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteJourneyCommand { JourneyId = journeyId, UserId = User.GetUserId() }, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Returns journey status.
    /// </summary>
    [HttpGet("{journeyId:guid}/status")]
    [ProducesResponseType(typeof(StatusViewModel<JourneyStatus>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StatusViewModel<JourneyStatus>>> GetStatus(Guid journeyId, CancellationToken cancellationToken)
    {
        var status = await _mediator.Send(new GetJourneyStatusCommand { JourneyId = journeyId, UserId = User.GetUserId() }, cancellationToken);

        return new StatusViewModel<JourneyStatus> { Id = journeyId, Status = status };
    }

    /// <summary>
    /// Setup progress: every step with errors, the next step and whether the journey can be approved.
    /// </summary>
    [HttpGet("{journeyId:guid}/progress")]
    [ProducesResponseType(typeof(JourneyProgressViewModel), StatusCodes.Status200OK)]
    public async Task<ActionResult<JourneyProgressViewModel>> GetProgress(Guid journeyId, CancellationToken cancellationToken)
    {
        var progress = await _mediator.Send(new GetJourneyProgressCommand { JourneyId = journeyId, UserId = User.GetUserId() }, cancellationToken);

        return _mapper.Map<JourneyProgressViewModel>(progress);
    }

    /// <summary>
    /// Step 2: journey dates. Days follow the period; trips of the remaining days are kept.
    /// </summary>
    [HttpPut("{journeyId:guid}/period")]
    [ProducesResponseType(typeof(JourneyViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public Task<ActionResult<JourneyViewModel>> SetPeriod(Guid journeyId, [FromBody] SetJourneyPeriodRequest request,
        CancellationToken cancellationToken) =>
        Journey(new SetJourneyPeriodCommand { JourneyId = journeyId, UserId = User.GetUserId(), Start = request.Start, End = request.End },
            cancellationToken);

    /// <summary>
    /// Step 3: journey country. It cannot be changed while hotels or day trips are selected.
    /// </summary>
    [HttpPut("{journeyId:guid}/country")]
    [ProducesResponseType(typeof(JourneyViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public Task<ActionResult<JourneyViewModel>> SelectCountry(Guid journeyId, [FromBody] SelectJourneyCountryRequest request,
        CancellationToken cancellationToken) =>
        Journey(new SelectJourneyCountryCommand { JourneyId = journeyId, UserId = User.GetUserId(), CountryId = request.CountryId },
            cancellationToken);

    /// <summary>
    /// Journey members (names).
    /// </summary>
    [HttpPut("{journeyId:guid}/members")]
    [ProducesResponseType(typeof(JourneyViewModel), StatusCodes.Status200OK)]
    public Task<ActionResult<JourneyViewModel>> SetMembers(Guid journeyId, [FromBody] SetJourneyMembersRequest request,
        CancellationToken cancellationToken) =>
        Journey(new SetJourneyMembersCommand { JourneyId = journeyId, UserId = User.GetUserId(), Members = request.Members },
            cancellationToken);

    /// <summary>
    /// Budget limit; the total is calculated from hotels, transports and trip activities.
    /// </summary>
    [HttpPut("{journeyId:guid}/budget")]
    [ProducesResponseType(typeof(JourneyViewModel), StatusCodes.Status200OK)]
    public Task<ActionResult<JourneyViewModel>> SetBudget(Guid journeyId, [FromBody] SetJourneyBudgetRequest request,
        CancellationToken cancellationToken) =>
        Journey(new SetJourneyBudgetCommand { JourneyId = journeyId, UserId = User.GetUserId(), Budget = request.Budget },
            cancellationToken);

    /// <summary>
    /// Hotels of the journey country available for the stay (defaults to the whole period), cheapest first.
    /// </summary>
    [HttpGet("{journeyId:guid}/hotels/available")]
    [ProducesResponseType(typeof(List<HotelOfferViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<HotelOfferViewModel>>> SearchHotels(Guid journeyId, [FromQuery] HotelSearchRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<SearchJourneyHotelsCommand>(request);
        command.JourneyId = journeyId;
        command.UserId = User.GetUserId();

        return _mapper.Map<List<HotelOfferViewModel>>(await _mediator.Send(command, cancellationToken));
    }

    /// <summary>
    /// Step 4: adds a hotel stay. Stays cover every night; a second stay starting later means a hotel switch.
    /// </summary>
    [HttpPost("{journeyId:guid}/hotels")]
    [ProducesResponseType(typeof(JourneyViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public Task<ActionResult<JourneyViewModel>> AddHotel(Guid journeyId, [FromBody] JourneyHotelRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<AddJourneyHotelCommand>(request);
        command.JourneyId = journeyId;
        command.UserId = User.GetUserId();

        return Journey(command, cancellationToken);
    }

    /// <summary>
    /// Changes dates / guests of a hotel stay.
    /// </summary>
    [HttpPut("{journeyId:guid}/hotels/{bookingId:guid}")]
    [ProducesResponseType(typeof(JourneyViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public Task<ActionResult<JourneyViewModel>> UpdateHotel(Guid journeyId, Guid bookingId, [FromBody] UpdateJourneyHotelRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<UpdateJourneyHotelCommand>(request);
        command.JourneyId = journeyId;
        command.BookingId = bookingId;
        command.UserId = User.GetUserId();

        return Journey(command, cancellationToken);
    }

    /// <summary>
    /// Removes a hotel stay (cancels the not paid booking).
    /// </summary>
    [HttpDelete("{journeyId:guid}/hotels/{bookingId:guid}")]
    [ProducesResponseType(typeof(JourneyViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public Task<ActionResult<JourneyViewModel>> RemoveHotel(Guid journeyId, Guid bookingId, CancellationToken cancellationToken) =>
        Journey(new RemoveJourneyHotelCommand { JourneyId = journeyId, BookingId = bookingId, UserId = User.GetUserId() }, cancellationToken);

    /// <summary>
    /// Trips which fit the day: from the hotel of the previous night to the hotel of the coming night.
    /// Create a new trip (POST /api/trips) when nothing fits.
    /// </summary>
    [HttpGet("{journeyId:guid}/days/{date:datetime}/trips")]
    [ProducesResponseType(typeof(List<TripViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<TripViewModel>>> GetDayTrips(Guid journeyId, DateTime date, CancellationToken cancellationToken)
    {
        var trips = await _mediator.Send(new GetDayTripsCommand { JourneyId = journeyId, Date = date, UserId = User.GetUserId() },
            cancellationToken);

        return _mapper.Map<List<TripViewModel>>(trips);
    }

    /// <summary>
    /// Step 5: schedules the trip on the day.
    /// </summary>
    [HttpPut("{journeyId:guid}/days/{date:datetime}")]
    [ProducesResponseType(typeof(JourneyViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public Task<ActionResult<JourneyViewModel>> AssignDayTrip(Guid journeyId, DateTime date, [FromBody] AssignDayTripRequest request,
        CancellationToken cancellationToken) =>
        Journey(new AssignDayTripCommand { JourneyId = journeyId, Date = date, TripId = request.TripId, UserId = User.GetUserId() },
            cancellationToken);

    /// <summary>
    /// Makes the day free (no trip).
    /// </summary>
    [HttpDelete("{journeyId:guid}/days/{date:datetime}")]
    [ProducesResponseType(typeof(JourneyViewModel), StatusCodes.Status200OK)]
    public Task<ActionResult<JourneyViewModel>> ClearDayTrip(Guid journeyId, DateTime date, CancellationToken cancellationToken) =>
        Journey(new ClearDayTripCommand { JourneyId = journeyId, Date = date, UserId = User.GetUserId() }, cancellationToken);

    /// <summary>
    /// Adds transport to the country / between cities (optional).
    /// </summary>
    [HttpPost("{journeyId:guid}/transports")]
    [ProducesResponseType(typeof(TransportViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<TransportViewModel>> AddTransport(Guid journeyId, [FromBody] AddTransportRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<AddJourneyTransportCommand>(request);
        command.JourneyId = journeyId;
        command.UserId = User.GetUserId();

        var transport = await _mediator.Send(command, cancellationToken);

        return StatusCode(StatusCodes.Status201Created, _mapper.Map<TransportViewModel>(transport));
    }

    /// <summary>
    /// Finishes the setup (Approved = true): all steps are validated, hotel bookings are frozen, edits are disabled.
    /// </summary>
    [HttpPost("{journeyId:guid}/approve")]
    [ProducesResponseType(typeof(JourneyViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public Task<ActionResult<JourneyViewModel>> Approve(Guid journeyId, CancellationToken cancellationToken) =>
        Journey(new ApproveJourneyCommand { JourneyId = journeyId, UserId = User.GetUserId() }, cancellationToken);

    /// <summary>
    /// Enables editing again (Approved = false). Possible only before the journey starts.
    /// </summary>
    [HttpPost("{journeyId:guid}/unapprove")]
    [ProducesResponseType(typeof(JourneyViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public Task<ActionResult<JourneyViewModel>> Unapprove(Guid journeyId, CancellationToken cancellationToken) =>
        Journey(new UnapproveJourneyCommand { JourneyId = journeyId, UserId = User.GetUserId() }, cancellationToken);

    private async Task<ActionResult<JourneyViewModel>> Journey(IRequest<JourneyModel> command, CancellationToken cancellationToken)
    {
        var journey = await _mediator.Send(command, cancellationToken);

        return _mapper.Map<JourneyViewModel>(journey);
    }
}
