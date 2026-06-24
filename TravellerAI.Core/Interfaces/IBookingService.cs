using TravellerAI.Domain.Models;
using TravellerAI.Domain.ViewModels;

namespace TravellerAI.Core.Interfaces;

public interface IBookingService
{
    Task<bool> CheckAvailableDates(JourneyModel journey, BookingViewModel booking);
    Task SelectPlace(JourneyModel journey, BookingViewModel booking);
    Task<string> AddDates();
    Task<bool> UpdateBookingAsync(BookingModel booking);
    Task <string> AddBankCard();
    Task <string> AddBooking();
    Task<bool> IsValidAsync();
}


