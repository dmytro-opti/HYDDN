using MediatR;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;
using TravellerAI.Domain.ViewModels;

namespace TravellerAI.Core.Features.AddTransportCommand;

public class AddTransportCommand : IRequest<TransportViewModel>
{
    public Guid TripId { get; set; }
    public Guid? JourneyId { get; set; }
    public TransportType Type { get; set; }
    public string Company { get; set; }
    public SeatClass SeatClass { get; set; }
    public int SeatCount { get; set; }
}