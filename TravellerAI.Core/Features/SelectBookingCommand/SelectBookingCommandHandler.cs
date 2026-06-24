using MediatR;
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
    
    public async Task<bool> Handle(SelectBookingCommand command, CancellationToken cancellationToken)
    {
        // check user availability
        var userModel = await _userService.GetUserAsync(command.UserId);
        if (userModel == null)
        {
            throw new Exception($"User {command.UserId} does not exist");
        }
        
        // check trip
        var tripModel = await _tripService.GetTripAsync(command.TripId);
        
        if (tripModel != null 
            && tripModel.User?.Id == command.UserId 
            && tripModel.Booking?.BookingId == command.BookingId)
        {
            var isValid = await _bookingService.IsValidAsync();

            if (isValid)
            {
                tripModel.Booking.IsFrozen = true;
            }

            return await _bookingService.UpdateBookingAsync(tripModel.Booking);
        }
        
        switch (tripModel)
        {
            case var x when x == null:
                throw new Exception($"Trip {command.TripId} does not exist");
            case var x when x.Booking == null:
                throw new Exception($"Booking {command.BookingId} for trip {tripModel.TripId} does not exist");
            case var x when x.User == null:
                throw new Exception($"User {command.UserId} for trip {tripModel.TripId} does not exist");
            default:
                throw new Exception("Unexpected error");
        }
    }
}