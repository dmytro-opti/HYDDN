using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Journeys.UpdateJourneyHotelCommand;

public class UpdateJourneyHotelCommandHandler : IRequestHandler<UpdateJourneyHotelCommand, JourneyModel>
{
    private readonly IJourneyService _journeyService;

    public UpdateJourneyHotelCommandHandler(IJourneyService journeyService)
    {
        _journeyService = journeyService;
    }

    public async Task<JourneyModel> Handle(UpdateJourneyHotelCommand command, CancellationToken cancellationToken)
    {
        return await _journeyService.UpdateHotelAsync(command.UserId, command.JourneyId, command.BookingId, command.CheckIn, command.CheckOut, command.Adults, command.Children);
    }
}
