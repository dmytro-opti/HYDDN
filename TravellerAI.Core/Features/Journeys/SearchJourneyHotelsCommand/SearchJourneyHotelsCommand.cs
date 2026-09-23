using MediatR;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Journeys.SearchJourneyHotelsCommand;

/// <summary>
/// Available hotels in the journey country for the stay (defaults to the whole period).
/// </summary>
public class SearchJourneyHotelsCommand : IRequest<List<HotelOfferModel>>
{
    public Guid UserId { get; set; }
    public Guid JourneyId { get; set; }
    public DateTime? CheckIn { get; set; }
    public DateTime? CheckOut { get; set; }
    public string? City { get; set; }
    public int Guests { get; set; }
}
