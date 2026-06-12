using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Exceptions;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.AddBookingCommand;

public class AddBookingCommandHandler : IRequestHandler<AddBookingCommand, Guid>
{
    private readonly IJourneyService _journeyService;
    private readonly IUserService _userService;
    
    public AddBookingCommandHandler(IJourneyService journeyService, IUserService userService)
    {
        _journeyService = journeyService;
        _userService = userService;
    }
    public async Task<Guid> Handle(AddBookingCommand command, CancellationToken cancellationToken)
    {
        // check user availability
        var user = await _userService.GetUser(command.UserId);
        if (user == null)
        {
            throw new Exception($"User {command.UserId} does not exist");
        }
        
        // check journey
        var journey = await _journeyService.GetJourney(command.JourneyId);
        if (journey == null)
        {
            throw new Exception($"Journey {command.JourneyId} does not exist");
        }
        
        // check property
        //bookingService
        
        // check room
        // bookingService
        
        // check guest count + checkin
        // bookingService
        
        // convert command model into bookingmodel
        // automapper
        
        // add booking model to journeymodel
        
        return Guid.NewGuid();
    }
}

