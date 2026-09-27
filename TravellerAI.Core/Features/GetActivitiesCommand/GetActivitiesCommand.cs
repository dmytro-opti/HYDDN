

using MediatR;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.GetActivitiesCommand;

public class GetActivitiesCommand : IRequest<List<ActivityModel>>
{ 
    public string Name { get; set; }
    public IEnumerable<ActivityType> Types { get; set; }
    public decimal PriceFrom { get; set; }
    public decimal PriceTo { get; set; }
    public Guid LocationId { get; set; }
    public IEnumerable<JourneyStatus> Statuses { get; set; }
    public decimal Rating { get; set; }
    public PeriodModel Period { get; set; }
}