using AutoMapper;
using TravellerAI.Core.Interfaces;
using TravellerAI.Core.Repositories;
using TravellerAI.Domain.Entities;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Exceptions;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Services;

public class TransportService : ITransportService
{
    private readonly ITransportRepository _transportRepository;
    private readonly ITripRepository _tripRepository;
    private readonly ILoggerService<TransportService> _logger;
    private readonly IMapper _mapper;

    public TransportService(ITransportRepository transportRepository, ITripRepository tripRepository,
        ILoggerService<TransportService> logger, IMapper mapper)
    {
        _transportRepository = transportRepository;
        _tripRepository = tripRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public Task<string> SearchTransport(TransportModel transport)
    {
        // requires an external transport provider
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns stored transports for the given ones (matched by Id).
    /// </summary>
    public async Task<List<TransportModel>> SelectTransports(List<TransportModel> transports)
    {
        var ids = transports.Select(t => t.Id).ToList();
        var entities = await _transportRepository.FindAsync(t => ids.Contains(t.Id));

        return _mapper.Map<List<TransportModel>>(entities);
    }

    /// <summary>
    /// Returns stored transports which have free seats and have not departed yet.
    /// </summary>
    public async Task<List<TransportModel>> SelectAvailableTransports(List<TransportModel> transports)
    {
        var selected = await SelectTransports(transports);
        var now = DateTime.UtcNow;

        return selected
            .Where(t => t.SeatCount > 0 && (t.Period == null || t.Period.Start > now))
            .ToList();
    }

    public async Task<TransportModel> AddTransportAsync(Guid TripId, Guid? JourneyId, TransportType Type, string Company, SeatClass SeatClass,
        int SeatCount)
    {
        if (!await _tripRepository.ExistsAsync(TripId))
        {
            throw new ResourceNotFoundException($"Trip {TripId} not found");
        }

        var transport = new TransportEntity
        {
            TripId = TripId,
            JourneyId = JourneyId,
            Type = Type,
            Company = Company,
            SeatClass = SeatClass,
            SeatCount = SeatCount
        };

        await _transportRepository.AddAsync(transport);
        _logger.Log(ErrorLevel.Low, $"Transport {transport.Id} was added to trip {TripId}");

        return _mapper.Map<TransportModel>(transport);
    }
}
