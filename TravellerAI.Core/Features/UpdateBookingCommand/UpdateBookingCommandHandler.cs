using AutoMapper;
using MediatR;
using TravellerAI.Core.Exceptions;
using TravellerAI.Core.Interfaces;

namespace TravellerAI.Core.Features.UpdateBookingCommand;

public class UpdateBookingCommandHandler : IRequestHandler<UpdateBookingCommand, bool>
{
    private readonly IBookingService _bookingService;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    
    public UpdateBookingCommandHandler(IBookingService bookingService, IUserService userService, IMapper mapper)
    {
        _bookingService = bookingService;
        _userService = userService;
        _mapper = mapper;
    }

    public async Task<bool> Handle(UpdateBookingCommand command, CancellationToken cancellationToken)
    {
        // both throw NotFoundException when missing
        await _userService.GetUserAsync(command.UserId);
        var booking = await _bookingService.GetBookingModelAsync(command.BookingId);

        if (booking.UserId != command.UserId)
        {
            throw new ForbiddenException($"Booking {command.BookingId} does not belong to user {command.UserId}");
        }

        _mapper.Map(command, booking);

        return await _bookingService.UpdateBookingAsync(booking);
    }
}
