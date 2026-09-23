using MediatR;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.UpdateTripCommand;

public class UpdateTripCommand : IRequest<TripModel>
{
    public Guid TripId { get; set; }
    public string Name  { get; set; }
    public GroupModel Group { get; set; }
    public BookingModel Booking { get; set; }
    public MapModel Map { get; set; }
    public PeriodModel Period { get; set; }
    public double Rating { get; set; }    
}