using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Journeys.RemoveJourneyHotelCommand;

public class RemoveJourneyHotelCommandHandler : IRequestHandler<RemoveJourneyHotelCommand, JourneyModel>
{
    private readonly IJourneyService _journeyService;

    public RemoveJourneyHotelCommandHandler(IJourneyService journeyService)
    {
        _journeyService = journeyService;
    }

    public async Task<JourneyModel> Handle(RemoveJourneyHotelCommand command, CancellationToken cancellationToken)
    {
        return await _journeyService.RemoveHotelAsync(command.UserId, command.JourneyId, command.BookingId);
    }
}
