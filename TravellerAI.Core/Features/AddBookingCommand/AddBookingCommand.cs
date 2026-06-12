using MediatR;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.AddBookingCommand;

public class AddBookingCommand : IRequest<Guid>
{
    public Guid UserId { get; set; }
    public Guid JourneyId { get; set; }
    public Guid PropertyId { get; set; }
    public Guid RoomId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int GuestCount { get; set; }
}