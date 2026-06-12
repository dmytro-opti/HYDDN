using MediatR;
using TravellerAI.Domain.Models;
using TravellerAI.Domain.ViewModels;

namespace TravellerAI.Core.Features.BuildTripCommand;

public class BuildTripCommand : IRequest<TripModel>
{
    public string Name  { get; set; }
    public UserModel User { get; set; }
    public GroupModel Group { get; set; }
    public PeriodViewModel Period { get; set; }
    public Guid TripId { get; set; }
    public int Budget { get; set; }
    public List<JourneyViewModel> Journeys { get; set; }
    public TransportViewModel Transport { get; set; }
    public BookingViewModel Booking { get; set; }
}