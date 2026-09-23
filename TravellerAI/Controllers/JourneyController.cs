using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravellerAI.Auth;
using TravellerAI.Core.Features.BuildJourneyCommand;
using TravellerAI.Core.Features.GetJourneyStatusCommand;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.ViewModels;
using TravellerAI.Domain.ViewModels.Requests;

namespace TravellerAI.WebApi.Controllers;

/// <summary>
/// Journeys - user travel plans consisting of trips.
/// </summary>
/// <remarks>
/// Exceptions are translated to HTTP responses by GlobalExceptionHandler.
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
    /// Creates a new active journey for the user. Returns the journey id.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateJourneyRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<BuildJourneyCommand>(request);
        command.UserId = User.GetUserId();

        var journeyId = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetStatus), new { journeyId }, journeyId);
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
}
