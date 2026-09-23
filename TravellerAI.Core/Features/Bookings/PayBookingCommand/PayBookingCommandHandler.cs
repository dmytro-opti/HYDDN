using MediatR;
using TravellerAI.Core.Exceptions;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Bookings.PayBookingCommand;

public class PayBookingCommandHandler : IRequestHandler<PayBookingCommand, BookingModel>
{
    private readonly IBookingService _bookingService;

    public PayBookingCommandHandler(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    public async Task<BookingModel> Handle(PayBookingCommand command, CancellationToken cancellationToken)
    {
        var booking = await _bookingService.GetBookingModelAsync(command.BookingId);
        if (booking.UserId != command.UserId)
        {
            throw new ForbiddenException($"Booking {command.BookingId} does not belong to user {command.UserId}");
        }

        return await _bookingService.PayAsync(command.BookingId, command.PaymentMethod);
    }
}
