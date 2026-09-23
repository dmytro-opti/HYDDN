using AutoMapper;
using TravellerAI.Core.Exceptions;
using TravellerAI.Core.Interfaces;
using TravellerAI.Core.Repositories;
using TravellerAI.Domain.Entities;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Services;

public class BudgetService : IBudgetService
{
    private readonly ITripRepository _tripRepository;
    private readonly IJourneyRepository _journeyRepository;
    private readonly ILoggerService<BudgetService> _logger;
    private readonly IMapper _mapper;

    public BudgetService(ITripRepository tripRepository, IJourneyRepository journeyRepository,
        ILoggerService<BudgetService> logger, IMapper mapper)
    {
        _tripRepository = tripRepository;
        _journeyRepository = journeyRepository;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// Sets budget limit of the trip.
    /// </summary>
    public async Task SetBudget(TripModel trip, int budget)
    {
        EnsureValid(budget);

        var entity = await _tripRepository.GetByIdAsync(trip.TripId)
                     ?? throw new NotFoundException("Trip", trip.TripId);

        entity.Budget ??= new BudgetEntity();
        entity.Budget.Budget = budget;
        await _tripRepository.UpdateAsync(entity);

        trip.Budget = _mapper.Map<BudgetModel>(entity.Budget);
        _logger.Log(ErrorLevel.Low, $"Trip {entity.Id} budget was set to {budget}");
    }

    /// <summary>
    /// Sets budget limit of the journey.
    /// </summary>
    public async Task UpdateBudget(JourneyModel journey, int budget)
    {
        EnsureValid(budget);

        var entity = await _journeyRepository.GetByIdAsync(journey.Id)
                     ?? throw new NotFoundException("Journey", journey.Id);

        entity.Budget ??= new BudgetEntity();
        entity.Budget.Budget = budget;
        await _journeyRepository.UpdateAsync(entity);

        journey.Budget = _mapper.Map<BudgetModel>(entity.Budget);
        _logger.Log(ErrorLevel.Low, $"Journey {entity.Id} budget was set to {budget}");
    }

    private static void EnsureValid(int budget)
    {
        if (budget < 0)
        {
            throw new BadRequestException("Budget cannot be negative");
        }
    }
}
