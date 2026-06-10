using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Exceptions;
using TravellerAI.Domain.Models;
using TravellerAI.Core.Features.AddBookingCommand;

namespace TravellerAI.Core.Features.BuildJourneyCommand;

public class BuildJourneyCommandHandler : IRequestHandler<BuildJourneyCommand, Guid>
{
    private readonly IJourneyService _journeyService;
    private readonly IUserService _userService;
    private readonly IMediator _mediator;
    
    public BuildJourneyCommandHandler(IJourneyService journeyService, IUserService userService,  IMediator mediator)
    {
        _journeyService = journeyService;
        _userService = userService;
        _mediator = mediator;
    }
    public async Task<Guid> Handle(BuildJourneyCommand command, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUser(command.UserId);
        if (user == null)
        {
            throw new Exception($"User {command.UserId} does not exist");
        }

        var members = await _userService.GetByMemberIds(command.MemberIds);
        if (members.Count() != command.MemberIds.Count())
        {
            throw new Exception($"One or more members do not exist");
        }
        
        var journey = new JourneyModel
        {
            JourneyId = Guid.NewGuid(),
            UserId = command.UserId,
            Title = command.Title,
            Description = command.Description,
            Members = members,
            Period = command.Period
        };
        
        await _journeyService.CreateJourney(journey);
        
        await _mediator.Send(new AddBookingCommand.AddBookingCommand
        {
            JourneyId = journey.JourneyId,
            UserId = command.UserId,
            PropertyId = command.Booking.PropertyId, 
            RoomId = command.Booking.RoomId,
            GuestCount = command.Booking.GuestCount,
            CheckInDate = command.Period.Start,
            CheckOutDate = command.Period.End
            
        }, cancellationToken);
        return journey.JourneyId;
    }
}
