using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.AddBookingCommand;

public class AddBookingCommandHandler : IRequestHandler<AddBookingCommand, bool>
{
    private readonly ITripService _tripService;
    private readonly IUserService _userService;
    
    public AddBookingCommandHandler(ITripService tripService, IUserService userService)
    {
        _tripService = tripService;
        _userService = userService;
    }
    public async Task<bool> Handle(AddBookingCommand command, CancellationToken cancellationToken)
    {
        // check user availability
        var user = await _userService.GetUserAsync(command.UserId);
        if (user == null)
        {
            throw new Exception($"User {command.UserId} does not exist");
        }
        
        // check trip
        var trip = await _tripService.GetTripAsync(command.TripId);
        if (trip == null)
        {
            throw new Exception($"Trip {command.TripId} does not exist");
        }

        trip.Booking = new BookingModel()
        {
            CheckInDate = command.Period.Start,
            CheckOutDate = command.Period.End,
            Status = BookingStatus.Pending
        };

        return await _tripService.UpdateTripAsync(trip);
    }
}