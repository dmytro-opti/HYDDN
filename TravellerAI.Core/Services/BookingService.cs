using AutoMapper;
using TravellerAI.Core.Exceptions;
using TravellerAI.Core.Interfaces;
using TravellerAI.Core.Repositories;
using TravellerAI.Domain.Entities;
using TravellerAI.Domain.Enums;
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
        var booking = await GetBookingEntityAsync(id);

        return _mapper.Map<BookingModel>(booking);
    }

    public Task<bool> CheckAvailableDates(JourneyModel journey, BookingViewModel booking)
    {
        // BookingViewModel does not carry place and dates yet
        throw new NotImplementedException();
    }

    public Task SelectPlace(JourneyModel journey, BookingViewModel booking)
    {
        // BookingViewModel does not carry place and dates yet
        throw new NotImplementedException();
    }

    public Task<string> AddDates()
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateBookingAsync(BookingModel booking)
    {
        var entity = await GetBookingEntityAsync(booking.BookingId);

        if (entity.IsFrozen)
        {
            throw new ConflictException($"Booking {entity.Id} is frozen and cannot be changed");
        }

        if (entity.Status is BookingStatus.Cancelled or BookingStatus.Completed)
        {
            throw new ConflictException($"Booking {entity.Id} is {entity.Status} and cannot be changed");
        }

        if (booking.CheckInDate != default && booking.CheckInDate >= booking.CheckOutDate)
        {
            throw new BadRequestException("Check-in date must be before check-out date");
        }

        if (booking.Status != BookingStatus.Cancelled && await HasOverlapAsync(booking))
        {
            throw new ConflictException($"Property {booking.PropertyId} is not available for the selected dates");
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

    /// <summary>
    /// Booking is valid for selection when it is still open, has correct future dates,
    /// at least one adult and its property is free for these dates.
    /// </summary>
    public async Task<bool> IsValidAsync(BookingModel booking)
    {
        var errors = new List<string>();

        if (booking.Status is not (BookingStatus.Pending or BookingStatus.Confirmed))
        {
            errors.Add($"status is {booking.Status}");
        }

        if (booking.CheckInDate >= booking.CheckOutDate)
        {
            errors.Add("check-in date is not before check-out date");
        }

        if (booking.CheckInDate < DateTime.UtcNow.Date)
        {
            errors.Add("check-in date is in the past");
        }

        if (booking.Adults < 1)
        {
            errors.Add("no adults");
        }

        if (errors.Count == 0 && await HasOverlapAsync(booking))
        {
            errors.Add("property is already booked for these dates");
        }

        if (errors.Count > 0)
        {
            _logger.Log(ErrorLevel.Medium, $"Booking {booking.BookingId} is not valid: {string.Join(", ", errors)}");
        }

        return errors.Count == 0;
    }

    private Task<bool> HasOverlapAsync(BookingModel booking)
    {
        return booking.PropertyId == Guid.Empty
            ? Task.FromResult(false)
            : _bookingRepository.HasOverlappingBookingAsync(
                booking.PropertyId, booking.CheckInDate, booking.CheckOutDate, booking.BookingId);
    }

    private async Task<BookingEntity> GetBookingEntityAsync(Guid id)
    {
        var booking = await _bookingRepository.GetByIdAsync(id);

        if (booking == null)
        {
            _logger.Log(ErrorLevel.Medium, $"Booking {id} not found");
            throw new NotFoundException("Booking", id);
        }

        return booking;
    }
}
