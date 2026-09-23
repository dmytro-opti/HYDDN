using AutoMapper;
using MediatR;
using TravellerAI.Core.Exceptions;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.AddBookingCommand;

public class AddBookingCommandHandler : IRequestHandler<AddBookingCommand, bool>
{
    private readonly ITripService _tripService;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    
    public AddBookingCommandHandler(ITripService tripService, IUserService userService, IMapper mapper)
    {
        _tripService = tripService;
        _userService = userService;
        _mapper = mapper;
    }

    public async Task<bool> Handle(AddBookingCommand command, CancellationToken cancellationToken)
    {
        // both throw NotFoundException when missing
        await _userService.GetUserAsync(command.UserId);
        var trip = await _tripService.GetTripAsync(command.TripId);

        if (trip.User?.Id != command.UserId)
        {
            throw new ForbiddenException($"Trip {command.TripId} does not belong to user {command.UserId}");
        }

        trip.Booking = _mapper.Map<BookingModel>(command);

        return await _tripService.UpdateTripAsync(trip);
    }
}
