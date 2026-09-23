using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravellerAI.Core.Features.Transports.SearchTransportsCommand;
using TravellerAI.Domain.ViewModels;
using TravellerAI.Domain.ViewModels.Requests;

namespace TravellerAI.WebApi.Controllers;

/// <summary>
/// Transports used in journeys.
/// </summary>
[ApiController]
[Authorize]
[Route("api/transports")]
[Produces("application/json")]
public class TransportController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public TransportController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Searches transports by type, class, company, price and departure, cheapest first.
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(List<TransportViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<TransportViewModel>>> Search([FromQuery] TransportSearchRequest request,
        CancellationToken cancellationToken)
    {
        var transports = await _mediator.Send(_mapper.Map<SearchTransportsCommand>(request), cancellationToken);

        return _mapper.Map<List<TransportViewModel>>(transports);
    }
}
