using MediatR;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Bookings.PayBookingCommand;

/// <summary>
/// Marks the booking as paid (card data is handled by a payment provider only).
/// </summary>
public class PayBookingCommand : IRequest<BookingModel>
{
    public Guid UserId { get; set; }
    public Guid BookingId { get; set; }
    public string PaymentMethod { get; set; }
}
