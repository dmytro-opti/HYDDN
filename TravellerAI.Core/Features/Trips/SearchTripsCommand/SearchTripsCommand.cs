using MediatR;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Trips.SearchTripsCommand;

/// <summary>
/// Public and own trips of the country, optionally from / to a location (hotel).
/// </summary>
public class SearchTripsCommand : IRequest<List<TripModel>>
{
    public Guid UserId { get; set; }
    public Guid CountryId { get; set; }
    public string? City { get; set; }
    public Guid? StartLocationId { get; set; }
    public Guid? EndLocationId { get; set; }
}
