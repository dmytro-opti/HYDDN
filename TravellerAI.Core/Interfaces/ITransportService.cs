using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;
using TravellerAI.Domain.ViewModels;

namespace TravellerAI.Core.Interfaces;

public interface ITransportService
{
    Task<string> SearchTransport(TransportModel transport);
    Task<List<TransportModel>> SelectTransports(List<TransportModel> transports);
    Task<List<TransportModel>> SelectAvailableTransports(List<TransportModel> transports);
    Task<TransportViewModel> AddTransportAsync(Guid TripId, Guid? JourneyId, TransportType Type, string Company, SeatClass SeatClass, int SeatCount);
}