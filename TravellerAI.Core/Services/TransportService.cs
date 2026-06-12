using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Services;

public class TransportService : ITransportService
{
    public Task<IEnumerable<TransportModel>> GetAvailableTransports(TransportType transportType, string Company)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<SeatModel>> CheckSeats(Guid TransportId, TransportType transportType, string Company, SeatClass seatClass)
    {
        throw new NotImplementedException();
    }
    
    public Task<Guid> CreateTransport(TransportModel transport)
    {
        throw new NotImplementedException();
    }
}