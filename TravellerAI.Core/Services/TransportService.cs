using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;
using TravellerAI.Domain.ViewModels;

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

    public Task<List<TransportModel>> SelectAvailableTransports()
    {
        throw new NotImplementedException();
    }

    public Task<TransportModel> AddTransportAsync(Guid TripId, Guid? JourneyId, TransportType Type, string Company, SeatClass SeatClass,
        int SeatCount)
    {
        throw new NotImplementedException();
    }
}
