using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Journeys.AddJourneyHotelCommand;

public class AddJourneyHotelCommandHandler : IRequestHandler<AddJourneyHotelCommand, JourneyModel>
{
    private readonly IJourneyService _journeyService;

    public AddJourneyHotelCommandHandler(IJourneyService journeyService)
    {
        _journeyService = journeyService;
    }

    public async Task<JourneyModel> Handle(AddJourneyHotelCommand command, CancellationToken cancellationToken)
    {
        return await _journeyService.AddHotelAsync(command.UserId, command.JourneyId, command.PlaceId, command.CheckIn, command.CheckOut, command.Adults, command.Children);
    }
}
