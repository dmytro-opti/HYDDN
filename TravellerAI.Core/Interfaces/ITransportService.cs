using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Interfaces;

public interface ITransportService
{
    Task<string> SearchTransport(TransportModel transport);
    Task<List<TransportModel>> SelectTransports(List<TransportModel> transports);
    Task<List<TransportModel>> SelectAvailableTransports();
}