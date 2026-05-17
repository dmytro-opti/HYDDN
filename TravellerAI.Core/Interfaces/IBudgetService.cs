using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Interfaces;

public interface IBudgetService
{
    Task SetBudget(TripModel trip, int budget);
    Task UpdateBudget(JourneyModel journey, int budget);
    
}