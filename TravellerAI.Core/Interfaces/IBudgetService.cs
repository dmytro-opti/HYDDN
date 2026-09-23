using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Interfaces;

public interface IBudgetService
{
    Task<BudgetModel> SetJourneyBudgetAsync(Guid journeyId, decimal budget);
    /// <summary>Total = hotel stays + transports + activities of the day trips (per member).</summary>
    Task<BudgetModel> RecalculateJourneyBudgetAsync(Guid journeyId);
}
