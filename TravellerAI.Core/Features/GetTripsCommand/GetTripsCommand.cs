using MediatR;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.GetTripsCommand;

public class GetTripsCommand : IRequest<IEnumerable<TripModel>>
{
    public Guid? UserId { get; set; }
    public PeriodModel? Period { get; set; }
    public double? PriceFrom { get; set; }
    public double?  PriceTo { get; set; }
    public Guid? LocationId { get; set; }
}