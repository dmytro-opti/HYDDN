using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Interfaces;

public interface ITransportService
{
    Task<IEnumerable<TransportModel>> GetAvailableTransports(TransportType transportType, string Company);
    Task<IEnumerable<SeatModel>> CheckSeats(Guid TransportId, TransportType transportType, string Company, SeatClass seatClass);
    Task<Guid> CreateTransport(TransportModel transport);
}