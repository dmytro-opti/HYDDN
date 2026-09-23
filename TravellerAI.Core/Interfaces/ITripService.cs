using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Interfaces;

/// <summary>
/// Catalog of one-day trip routes.
/// </summary>
public interface ITripService
{
    /// <summary>Validates stops (same country and city, distances) and saves the route.</summary>
    Task<TripModel> CreateTripAsync(Guid userId, TripDraftModel draft);
    /// <summary>Author only; trips used by other users or approved journeys cannot be changed.</summary>
    Task<TripModel> UpdateTripAsync(Guid userId, Guid tripId, TripDraftModel draft);
    /// <summary>Author only; trips scheduled in journeys cannot be deleted.</summary>
    Task DeleteTripAsync(Guid userId, Guid tripId);
    /// <summary>Public trips or own trips.</summary>
    Task<TripModel> GetTripAsync(Guid userId, Guid tripId);
    Task<IReadOnlyList<TripModel>> SearchTripsAsync(Guid userId, Guid countryId, string? city, Guid? startLocationId, Guid? endLocationId);
}
