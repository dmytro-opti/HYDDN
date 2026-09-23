using MediatR;
using TravellerAI.Core.Exceptions;
using TravellerAI.Core.Interfaces;

namespace TravellerAI.Core.Features.SelectBookingCommand;

public class SelectBookingCommandHandler : IRequestHandler<SelectBookingCommand, bool>
{
    private readonly ITripService _tripService;
    private readonly IUserService _userService;
    private readonly IBookingService _bookingService;
    
    public SelectBookingCommandHandler(ITripService tripService, 
        IUserService userService, IBookingService bookingService)
    {
        _userService = userService;
        _tripService = tripService;
        _bookingService = bookingService;
    }
    
    /// <summary>
    /// Freezes the trip booking when it is valid. Returns false when the booking is not valid.
    /// </summary>
    public async Task<bool> Handle(SelectBookingCommand command, CancellationToken cancellationToken)
    {
        // both throw NotFoundException when missing
        await _userService.GetUserAsync(command.UserId);
        var tripModel = await _tripService.GetTripAsync(command.TripId);

        if (tripModel.User?.Id != command.UserId)
        {
            throw new ForbiddenException($"Trip {command.TripId} does not belong to user {command.UserId}");
        }

        var booking = tripModel.Booking;
        if (booking == null || booking.BookingId != command.BookingId)
        {
            throw new NotFoundException($"Booking {command.BookingId} for trip {command.TripId} not found");
        }

        if (booking.IsFrozen)
        {
            throw new ConflictException($"Booking {command.BookingId} has already been selected");
        }

        if (!await _bookingService.IsValidAsync(booking))
        {
            return false;
        }

        booking.IsFrozen = true;

        return await _bookingService.UpdateBookingAsync(booking);
    }
}
