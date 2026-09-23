using MediatR;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Bookings.GetBookingCommand;

public class GetBookingCommand : IRequest<BookingModel>
{
    public Guid UserId { get; set; }
    public Guid BookingId { get; set; }
}
