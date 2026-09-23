using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Transports.SearchTransportsCommand;

public class SearchTransportsCommandHandler : IRequestHandler<SearchTransportsCommand, List<TransportModel>>
{
    private readonly ITransportService _transportService;

    public SearchTransportsCommandHandler(ITransportService transportService)
    {
        _transportService = transportService;
    }

    public async Task<List<TransportModel>> Handle(SearchTransportsCommand command, CancellationToken cancellationToken)
    {
        return await _transportService.SearchTransportsAsync(new TransportSearchModel
        {
            Type = command.Type,
            SeatClass = command.SeatClass,
            Company = command.Company,
            MaxPrice = command.MaxPrice,
            MinSeats = command.MinSeats,
            DepartureFrom = command.DepartureFrom,
            DepartureTo = command.DepartureTo
        });
    }
}
