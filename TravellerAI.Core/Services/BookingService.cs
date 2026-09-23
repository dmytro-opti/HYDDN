using AutoMapper;
using TravellerAI.Core.Exceptions;
using TravellerAI.Core.Interfaces;
using TravellerAI.Core.Repositories;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Exceptions;
using TravellerAI.Domain.Models;
using TravellerAI.Domain.ViewModels;

namespace TravellerAI.Core.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly ILoggerService<BookingService> _logger;
    private readonly IMapper _mapper;

    public BookingService(IBookingRepository bookingRepository, ILoggerService<BookingService> logger, IMapper mapper)
    {
        _bookingRepository = bookingRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<BookingModel> GetBookingModelAsync(Guid id)
    {
        var booking = await _bookingRepository.GetByIdAsync(id);

        if (booking == null)
        {
            _logger.Log(ErrorLevel.Medium, $"Booking {id} not found");
            throw new ResourceNotFoundException($"Booking {id} not found");
        }

        return _mapper.Map<BookingModel>(booking);
    }

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

    public async Task<bool> UpdateBookingAsync(BookingModel booking)
    {
        var entity = await _bookingRepository.GetByIdAsync(booking.BookingId)
                     ?? throw new ResourceNotFoundException($"Booking {booking.BookingId} not found");

        if (entity.IsFrozen)
        {
            _logger.Log(ErrorLevel.Medium, $"Booking {entity.Id} is frozen and cannot be changed");
            return false;
        }

        if (entity.Status is BookingStatus.Cancelled or BookingStatus.Completed)
        {
            _logger.Log(ErrorLevel.Medium, $"Booking {entity.Id} is {entity.Status} and cannot be changed");
            return false;
        }

        if (booking.CheckInDate != default && booking.CheckInDate >= booking.CheckOutDate)
        {
            throw new BadRequestException("Check-in date must be before check-out date");
        }

        if (booking.PropertyId != Guid.Empty
            && booking.Status != BookingStatus.Cancelled
            && await _bookingRepository.HasOverlappingBookingAsync(
                booking.PropertyId, booking.CheckInDate, booking.CheckOutDate, entity.Id))
        {
            _logger.Log(ErrorLevel.Medium, $"Property {booking.PropertyId} is not available for booking {entity.Id} dates");
            return false;
        }

        _mapper.Map(booking, entity);
        await _bookingRepository.UpdateAsync(entity);
        _logger.Log(ErrorLevel.Low, $"Booking {entity.Id} was updated");

        return true;
    }

    public Task<string> AddBankCard()
    {
        throw new NotImplementedException();
    }

    public Task<string> AddBooking()
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsValidAsync()
    {
        throw new NotImplementedException();
    }
}
