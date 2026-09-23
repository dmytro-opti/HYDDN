using MediatR;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Catalog.GetCountryLocationsCommand;

/// <summary>
/// Locations with coordinates which can be trip stops.
/// </summary>
public class GetCountryLocationsCommand : IRequest<List<LocationModel>>
{
    public Guid CountryId { get; set; }
    public string? City { get; set; }
}
