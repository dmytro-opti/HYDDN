using MediatR;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;
using TravellerAI.Domain.ViewModels;

namespace TravellerAI.Core.Features.CreateJourneyCommand;

public class CreateJourneyCommand : IRequest<JourneyViewModel>
{
    public JourneyViewModel Journey { get; set; }
    public JourneyStatus Status { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public UserModel User { get; set; }
    public IEnumerable<TripModel> Trips { get; set; }
    


}