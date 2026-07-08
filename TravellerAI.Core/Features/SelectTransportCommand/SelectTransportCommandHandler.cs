using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.SelectTransportCommand;

public class SelectTransportCommandHandler : IRequestHandler<SelectTransportCommand, TransportModel>
{
    private readonly ITransportService _transportService;
    
    public SelectTransportCommandHandler(ITransportService transportService)
    {
        _transportService = transportService;
    }
    
    public async Task<TransportModel> Handle(SelectTransportCommand command, CancellationToken cancellationToken)
    {
        var transport = await _transportService.SelectTransportAsync(command.TransportId);
        if (transport == null)
        {
            throw new Exception($"Transport {command.TransportId} could not be found.");
        }
        return transport;
    }
}