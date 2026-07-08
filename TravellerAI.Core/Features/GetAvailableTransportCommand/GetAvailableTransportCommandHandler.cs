using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.GetAvailableTransportCommand;

public class GetAvailableTransportCommandHandler : IRequestHandler<GetAvailableTransportCommand, List<TransportModel>>
{
    private readonly ITransportService _transportService;
    
    public GetAvailableTransportCommandHandler(ITransportService transportService)
    {
        _transportService = transportService;
    }
    public async Task<List<TransportModel>> Handle(GetAvailableTransportCommand command, CancellationToken cancellationToken)
    {
        var transports = await _transportService.SelectAvailableTransports();
        return transports.Where(t => t.IsAvailable == true).ToList();
    }
}