using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravellerAI.Core.Features.Catalog.GetCountriesCommand;
using TravellerAI.Core.Features.Catalog.GetCountryActivitiesCommand;
using TravellerAI.Core.Features.Catalog.GetCountryLocationsCommand;
using TravellerAI.Domain.ViewModels;

namespace TravellerAI.WebApi.Controllers;

/// <summary>
/// Countries and the places inside them used to build journeys and trips.
/// </summary>
[ApiController]
[AllowAnonymous]
[Route("api/countries")]
[Produces("application/json")]
public class CountryController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CountryController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Countries a journey can be planned in.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<CountryViewModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CountryViewModel>>> GetAll(CancellationToken cancellationToken)
    {
        return _mapper.Map<List<CountryViewModel>>(await _mediator.Send(new GetCountriesCommand(), cancellationToken));
    }

    /// <summary>
    /// Locations with coordinates (possible trip stops), optionally in the city.
    /// </summary>
    [HttpGet("{countryId:guid}/locations")]
    [ProducesResponseType(typeof(List<LocationViewModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<LocationViewModel>>> GetLocations(Guid countryId, [FromQuery] string? city,
        CancellationToken cancellationToken)
    {
        var locations = await _mediator.Send(new GetCountryLocationsCommand { CountryId = countryId, City = city }, cancellationToken);

        return _mapper.Map<List<LocationViewModel>>(locations);
    }

    /// <summary>
    /// Activities with an address (possible trip stops), optionally in the city.
    /// </summary>
    [HttpGet("{countryId:guid}/activities")]
    [ProducesResponseType(typeof(List<ActivityViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<ActivityViewModel>>> GetActivities(Guid countryId, [FromQuery] string? city,
        CancellationToken cancellationToken)
    {
        var activities = await _mediator.Send(new GetCountryActivitiesCommand { CountryId = countryId, City = city }, cancellationToken);

        return _mapper.Map<List<ActivityViewModel>>(activities);
    }
}
