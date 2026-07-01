using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;
using TravellerAI.Domain.ViewModels;

namespace TravellerAI.Core.Features.AddTransportCommand;

public class AddTransportCommandHandler : IRequestHandler<AddTransportCommand, TransportViewModel>
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
    public async Task<TransportViewModel> Handle(AddTransportCommand command, CancellationToken cancellationToken)
    {
        var trip = await _tripService.GetTripAsync(command.TripId);
        if (trip == null)
        {
            throw new Exception($"Trip with id {command.TripId} not found");
        }
        
        if (command.JourneyId.HasValue)
        {
            var journey = await _journeyService.GetJourneyAsync(command.JourneyId.Value);
            if (journey == null)
            {
                throw new Exception($"Journey with id {command.JourneyId} not found");
            }
        }
        
        return await _transportService.AddTransportAsync(command.TripId, command.JourneyId, command.Type, command.Company, command.SeatClass, command.SeatCount);
    }
}