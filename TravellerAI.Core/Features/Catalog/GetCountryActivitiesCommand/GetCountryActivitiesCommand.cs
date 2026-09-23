using MediatR;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Catalog.GetCountryActivitiesCommand;

/// <summary>
/// Activities with an address which can be trip stops.
/// </summary>
public class GetCountryActivitiesCommand : IRequest<List<ActivityModel>>
{
    public Guid CountryId { get; set; }
    public string? City { get; set; }
}
