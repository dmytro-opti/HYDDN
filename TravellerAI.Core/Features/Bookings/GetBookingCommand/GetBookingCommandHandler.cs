using MediatR;
using TravellerAI.Core.Exceptions;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Bookings.GetBookingCommand;

public class GetBookingCommandHandler : IRequestHandler<GetBookingCommand, BookingModel>
{
    private readonly IBookingService _bookingService;

    public GetBookingCommandHandler(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    public async Task<BookingModel> Handle(GetBookingCommand command, CancellationToken cancellationToken)
    {
        var booking = await _bookingService.GetBookingModelAsync(command.BookingId);

        return booking.UserId == command.UserId
            ? booking
            : throw new ForbiddenException($"Booking {command.BookingId} does not belong to user {command.UserId}");
    }
}
