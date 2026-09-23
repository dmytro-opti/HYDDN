using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Interfaces;

/// <summary>
/// Step by step journey setup. Mutating methods require the owner and a not approved journey.
/// </summary>
public interface IJourneyService
{
    Task<Guid> CreateJourneyAsync(Guid userId, string title, string? description, IEnumerable<string>? members);
    Task<IReadOnlyList<JourneyModel>> GetUserJourneysAsync(Guid userId, bool? approved = null);
    Task<JourneyModel> GetJourneyAsync(Guid userId, Guid journeyId);
    Task DeleteJourneyAsync(Guid userId, Guid journeyId);
    Task<JourneyProgressModel> GetProgressAsync(Guid userId, Guid journeyId);

    Task<JourneyModel> SetPeriodAsync(Guid userId, Guid journeyId, DateTime start, DateTime end);
    Task<JourneyModel> SelectCountryAsync(Guid userId, Guid journeyId, Guid countryId);
    Task<JourneyModel> SetMembersAsync(Guid userId, Guid journeyId, IEnumerable<string> members);
    Task<JourneyModel> SetBudgetAsync(Guid userId, Guid journeyId, decimal budget);

    Task<IReadOnlyList<HotelOfferModel>> SearchHotelsAsync(Guid userId, Guid journeyId, DateTime? checkIn, DateTime? checkOut,
        string? city, int guests);
    Task<JourneyModel> AddHotelAsync(Guid userId, Guid journeyId, Guid placeId, DateTime checkIn, DateTime checkOut, int adults, int children);
    Task<JourneyModel> UpdateHotelAsync(Guid userId, Guid journeyId, Guid bookingId, DateTime checkIn, DateTime checkOut, int adults, int children);
    Task<JourneyModel> RemoveHotelAsync(Guid userId, Guid journeyId, Guid bookingId);

    /// <summary>Trips which fit the day: in the journey country, from the day start hotel to the day end hotel.</summary>
    Task<IReadOnlyList<TripModel>> GetAvailableDayTripsAsync(Guid userId, Guid journeyId, DateTime date);
    Task<JourneyModel> AssignDayTripAsync(Guid userId, Guid journeyId, DateTime date, Guid tripId);
    Task<JourneyModel> ClearDayTripAsync(Guid userId, Guid journeyId, DateTime date);

    /// <summary>Finishes the setup: validates every step, freezes hotel bookings, disables edits.</summary>
    Task<JourneyModel> ApproveAsync(Guid userId, Guid journeyId);
    /// <summary>Enables editing again (only before the journey starts).</summary>
    Task<JourneyModel> UnapproveAsync(Guid userId, Guid journeyId);

    Task<JourneyStatus> GetJourneyStatusAsync(Guid journeyId);
    /// <summary>Throws NotFoundException / ForbiddenException when the journey does not belong to the user.</summary>
    Task EnsureOwnerAsync(Guid journeyId, Guid userId);
    /// <summary>Owner check + the journey is not approved and not cancelled.</summary>
    Task EnsureEditableAsync(Guid journeyId, Guid userId);
}
