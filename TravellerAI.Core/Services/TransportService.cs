using AutoMapper;
using TravellerAI.Core.Interfaces;
using TravellerAI.Core.Repositories;
using TravellerAI.Domain.Entities;
using TravellerAI.Domain.Entities.Owned;
using TravellerAI.Domain.Enums;
using TravellerAI.Core.Exceptions;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Services;

public class TransportService : ITransportService
{
    private readonly ITransportRepository _transportRepository;
    private readonly IJourneyRepository _journeyRepository;
    private readonly ILoggerService<TransportService> _logger;
    private readonly IMapper _mapper;

    public TransportService(ITransportRepository transportRepository, IJourneyRepository journeyRepository,
        ILoggerService<TransportService> logger, IMapper mapper)
    {
        _transportRepository = transportRepository;
        _journeyRepository = journeyRepository;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// Searches stored transports of not cancelled journeys by the criteria, cheapest first.
    /// </summary>
    public async Task<List<TransportModel>> SearchTransportsAsync(TransportSearchModel criteria)
    {
        if (criteria.DepartureFrom > criteria.DepartureTo)
        {
            throw new BadRequestException("DepartureFrom must be before DepartureTo");
        }

        var company = string.IsNullOrWhiteSpace(criteria.Company) ? null : criteria.Company.Trim();

        var transports = await _transportRepository.FindAsync(t =>
            t.Journey.Status != JourneyStatus.Cancelled
            && (criteria.Type == null || t.Type == criteria.Type)
            && (criteria.SeatClass == null || t.SeatClass == criteria.SeatClass)
            && (company == null || t.Company.Contains(company))
            && (criteria.MaxPrice == null || t.Price <= criteria.MaxPrice)
            && t.SeatCount >= criteria.MinSeats
            && (criteria.DepartureFrom == null || (t.Period != null && t.Period.Start >= criteria.DepartureFrom))
            && (criteria.DepartureTo == null || (t.Period != null && t.Period.Start <= criteria.DepartureTo)));

        return _mapper.Map<List<TransportModel>>(transports.OrderBy(t => t.Price).ThenBy(t => t.Period?.Start));
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

    public async Task<TransportModel> AddTransportAsync(Guid journeyId, TransportType type, string company, SeatClass seatClass,
        int seatCount, decimal price = 0, PeriodModel? period = null)
    {
        if (price < 0)
        {
            throw new BadRequestException("Transport price cannot be negative");
        }

        if (period != null && period.Start >= period.End)
        {
            throw new BadRequestException("Departure has to be before arrival");
        }

        var journey = await _journeyRepository.GetByIdAsync(journeyId)
                      ?? throw new NotFoundException("Journey", journeyId);

        if (journey.Status == JourneyStatus.Cancelled)
        {
            throw new ConflictException($"Journey {journeyId} is cancelled, transport cannot be added");
        }

        var transport = new TransportEntity
        {
            JourneyId = journeyId,
            Type = type,
            Company = company.Trim(),
            SeatClass = seatClass,
            SeatCount = seatCount,
            Price = price,
            Period = period == null ? null : new Period { Start = period.Start, End = period.End },
            Duration = period == null ? TimeSpan.Zero : period.End - period.Start
        };

        await _transportRepository.AddAsync(transport);
        _logger.Log(ErrorLevel.Low, $"Transport {transport.Id} was added to journey {journeyId}");

        return _mapper.Map<TransportModel>(transport);
    }
}
