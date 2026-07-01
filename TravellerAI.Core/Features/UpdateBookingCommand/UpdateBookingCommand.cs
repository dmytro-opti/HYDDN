using MediatR;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.UpdateBookingCommand;

public class UpdateBookingCommand : IRequest<bool>
{
    public Guid UserId { get; set; }
    
    public Guid BookingId { get; set; }
    public Guid PropertyId { get; set; }
    public Guid RoomId { get; set; }
    public string PaymentMethod { get; set; }
    public PeriodModel Period { get; set; }
    
    public int Children { get; set; }
    
    public int Adults { get; set; }
}