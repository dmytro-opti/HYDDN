using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TravellerAI.Core.Features.GetAvailableLocationCommand;
using TravellerAI.Domain.ViewModels;
using TravellerAI.Domain.ViewModels.Requests;

namespace TravellerAI.WebApi.Controllers;

/// <summary>
/// Available locations.
/// </summary>
/// <remarks>
/// Exceptions are translated to HTTP responses by GlobalExceptionHandler.
/// </remarks>
[ApiController]
[Route("api/locations")]
[Produces("application/json")]
public class LocationController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public LocationController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Returns locations of the country, optionally narrowed to the city.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<LocationViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<LocationViewModel>>> GetAvailable([FromQuery] LocationSearchRequest request,
        CancellationToken cancellationToken)
    {
        var locations = await _mediator.Send(_mapper.Map<GetAvailableLocationListCommand>(request), cancellationToken);

        return _mapper.Map<List<LocationViewModel>>(locations);
    }
}
