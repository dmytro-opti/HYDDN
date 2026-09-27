using MediatR;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.GetJourneysCommand;

public class GetJourneysCommand : IRequest<List<JourneyModel>>

{
    public IEnumerable<JourneyStatus> Statuses { get; set; }
    public string Title { get; set; }
}