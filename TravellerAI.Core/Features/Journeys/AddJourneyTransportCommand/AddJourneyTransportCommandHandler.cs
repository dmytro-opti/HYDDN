using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Journeys.AddJourneyTransportCommand;

public class AddJourneyTransportCommandHandler : IRequestHandler<AddJourneyTransportCommand, TransportModel>
{
    private readonly IJourneyService _journeyService;
    private readonly ITransportService _transportService;

    public AddJourneyTransportCommandHandler(IJourneyService journeyService, ITransportService transportService)
    {
        _journeyService = journeyService;
        _transportService = transportService;
    }

    public async Task<TransportModel> Handle(AddJourneyTransportCommand command, CancellationToken cancellationToken)
    {
        await _journeyService.EnsureEditableAsync(command.JourneyId, command.UserId);

        return await _transportService.AddTransportAsync(command.JourneyId, command.Type, command.Company, command.SeatClass,
            command.SeatCount, command.Price, command.Period);
    }
}
