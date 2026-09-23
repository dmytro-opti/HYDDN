using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Interfaces;

public interface ITransportService
{
    Task<List<TransportModel>> SearchTransportsAsync(TransportSearchModel criteria);
    Task<List<TransportModel>> SelectTransports(List<TransportModel> transports);
    Task<List<TransportModel>> SelectAvailableTransports(List<TransportModel> transports);
    Task<TransportModel> AddTransportAsync(Guid journeyId, TransportType type, string company, SeatClass seatClass, int seatCount,
        decimal price = 0, PeriodModel? period = null);
}
