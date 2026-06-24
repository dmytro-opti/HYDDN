using MediatR;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.SelectBookingCommand;

public class SelectBookingCommand : IRequest<bool>
{
    public Guid BookingId { get; set; }
    public Guid TripId { get; set; }
    public Guid UserId { get; set; }
}