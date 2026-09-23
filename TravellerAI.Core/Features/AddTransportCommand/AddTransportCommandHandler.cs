using MediatR;
using TravellerAI.Core.Exceptions;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.AddTransportCommand;

public class AddTransportCommandHandler : IRequestHandler<AddTransportCommand, TransportModel>
{
    private readonly ITransportService _transportService;
    private readonly ITripService _tripService;
    private readonly IJourneyService _journeyService;
    
    public AddTransportCommandHandler(ITransportService transportService, ITripService tripService, IJourneyService journeyService)
    {
        _transportService = transportService;
        _tripService = tripService;
        _journeyService = journeyService;
    }

    public async Task<TransportModel> Handle(AddTransportCommand command, CancellationToken cancellationToken)
    {
        // throws NotFoundException when missing
        var trip = await _tripService.GetTripAsync(command.TripId);
        
        if (command.JourneyId.HasValue)
        {
            var journey = await _journeyService.GetJourneyAsync(command.JourneyId.Value);

            if (journey.User?.Id != trip.User?.Id)
            {
                throw new BadRequestException($"Journey {journey.Id} and trip {trip.TripId} belong to different users");
            }
        }
        
        return await _transportService.AddTransportAsync(command.TripId, command.JourneyId, command.Type, command.Company, command.SeatClass, command.SeatCount, command.Price);
    }
}
