using AutoMapper;
using TravellerAI.Core.Exceptions;
using TravellerAI.Core.Interfaces;
using TravellerAI.Core.Journeys;
using TravellerAI.Core.Repositories;
using TravellerAI.Domain.Entities;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Services;

public class BudgetService : IBudgetService
{
    private readonly IJourneyRepository _journeyRepository;
    private readonly ILoggerService<BudgetService> _logger;
    private readonly IMapper _mapper;

    public BudgetService(IJourneyRepository journeyRepository, ILoggerService<BudgetService> logger, IMapper mapper)
    {
        _journeyRepository = journeyRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<BudgetModel> SetJourneyBudgetAsync(Guid journeyId, decimal budget)
    {
        if (budget < Constants.Validation.MinBudget)
        {
            throw new BadRequestException("Budget cannot be negative");
        }

        var journey = await GetJourneyAsync(journeyId);
        journey.Budget ??= new BudgetEntity();
        journey.Budget.Budget = budget;
        journey.Budget.Total = CalculateTotal(journey);

        await _journeyRepository.UpdateAsync(journey);

        return _mapper.Map<BudgetModel>(journey.Budget);
    }

    public async Task<BudgetModel> RecalculateJourneyBudgetAsync(Guid journeyId)
    {
        var journey = await GetJourneyAsync(journeyId);
        journey.Budget ??= new BudgetEntity();
        journey.Budget.Total = CalculateTotal(journey);

        await _journeyRepository.UpdateAsync(journey);

        if (journey.Budget.Budget > 0 && journey.Budget.Total > journey.Budget.Budget)
        {
            _logger.Log(ErrorLevel.Medium, $"Journey {journeyId} exceeds its budget: {journey.Budget.Total} > {journey.Budget.Budget}");
        }

        return _mapper.Map<BudgetModel>(journey.Budget);
    }

    /// <summary>
    /// Hotel stays + transports + activities of the scheduled trips for every member (at least one traveller).
    /// </summary>
    private static decimal CalculateTotal(JourneyEntity journey)
    {
        var travellers = Math.Max(1, journey.Members.Count);

        var hotels = JourneyPlan.ActiveStays(journey).Sum(s => s.TotalPrice);
        var transports = journey.Transports.Sum(t => t.Price);
        var activities = journey.Days
            .Where(d => d.Trip != null)
            .SelectMany(d => d.Trip!.Stops)
            .Sum(s => s.Activity?.Price ?? 0) * travellers;

        return hotels + transports + activities;
    }

    private async Task<JourneyEntity> GetJourneyAsync(Guid journeyId) =>
        await _journeyRepository.GetByIdAsync(journeyId) ?? throw new NotFoundException("Journey", journeyId);
}
