using MediatR;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.CreateJourneyCommand;

public class CreateJourneyCommand : IRequest<Guid>
{
    public Guid UserId { get; set; }
    public PeriodModel Period { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}