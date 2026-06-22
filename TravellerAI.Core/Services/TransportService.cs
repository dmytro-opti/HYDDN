using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Services;

public class TransportService : ITransportService
{
    public Task<string> SearchTransport(TransportModel transport)
    {
        throw new NotImplementedException();
    }

    public Task<List<TransportModel>> SelectTransports(List<TransportModel> transports)
    {
        throw new NotImplementedException();
    }

    public Task<List<TransportModel>> SelectAvailableTransports(List<TransportModel> transports)
    {
        throw new NotImplementedException();
    }
}
