
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;
using TravellerAI.Domain.ViewModels;

namespace TravellerAI.Core.Services;

public class BookingService : IBookingService
{
    public Task<bool> CheckAvailableDates(JourneyModel journey, BookingViewModel booking)
    {
        throw new NotImplementedException();
    }

    public Task SelectPlace(JourneyModel journey, BookingViewModel booking)
    {
        throw new NotImplementedException();
    }

    public Task<string> AddDates()
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateBooking()
    {
        throw new NotImplementedException();
    }

    public Task<string> AddBankCard()
    {
        throw new NotImplementedException();
    }

    public Task<string> AddBooking()
    {
        throw new NotImplementedException();
    }
}