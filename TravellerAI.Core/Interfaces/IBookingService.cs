using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Interfaces;

public interface IBookingService
{
    Task<BookingModel> GetBookingModelAsync(Guid id);
    Task<bool> IsPlaceAvailableAsync(Guid placeId, DateTime checkIn, DateTime checkOut, Guid? exceptBookingId = null);
    /// <summary>Creates a pending booking of an available place; price is calculated from the place rate.</summary>
    Task<BookingModel> CreateBookingAsync(BookingModel booking);
    /// <summary>Changes dates (and guests) of a not paid, not frozen booking; price is recalculated.</summary>
    Task<BookingModel> ChangeDatesAsync(Guid bookingId, PeriodModel period, int? adults = null, int? children = null);
    /// <summary>Cancels a not paid booking.</summary>
    Task CancelAsync(Guid bookingId);
    /// <summary>Marks the booking as paid and confirmed. Card data is never stored.</summary>
    Task<BookingModel> PayAsync(Guid bookingId, string paymentMethod);
}
