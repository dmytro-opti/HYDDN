using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.UpdateBookingCommand;

public class UpdateBookingCommandHandler : IRequestHandler<UpdateBookingCommand, bool>
{
    private readonly IBookingService _bookingService;
    private readonly IUserService _userService;
    
    public UpdateBookingCommandHandler(IBookingService bookingService, IUserService userService)
    {
        _bookingService = bookingService;
        _userService = userService;
    }
    public async Task<bool> Handle(UpdateBookingCommand command, CancellationToken cancellationToken)
    {
        // check user availability
        var user = await _userService.GetUserAsync(command.UserId);
        if (user == null)
        {
            throw new Exception($"User {command.UserId} does not exist");
        }
        
        // check booking
        var booking = await _bookingService.GetBookingModelAsync(command.BookingId);
        if (booking == null)
        {
            throw new Exception($"Booking {command.BookingId} does not exist");
        }

        booking.PropertyId = command.PropertyId;
        booking.RoomId = command.RoomId;
        booking.PaymentMethod = command.PaymentMethod;
        booking.CheckInDate = command.Period.Start;
        booking.CheckOutDate = command.Period.End;
        booking.Adults = command.Adults;
        booking.Children = command.Children;
        

        return await _bookingService.UpdateBookingAsync(booking);
    }
}