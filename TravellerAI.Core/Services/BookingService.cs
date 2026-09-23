using AutoMapper;
using TravellerAI.Core.Exceptions;
using TravellerAI.Core.Interfaces;
using TravellerAI.Core.Repositories;
using TravellerAI.Domain.Entities;
using TravellerAI.Domain.Entities.Owned;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IRepository<PlaceEntity> _placeRepository;
    private readonly IUserRepository _userRepository;
    private readonly INotificationService _notificationService;
    private readonly ILoggerService<BookingService> _logger;
    private readonly IMapper _mapper;

    public BookingService(IBookingRepository bookingRepository, IRepository<PlaceEntity> placeRepository,
        IUserRepository userRepository, INotificationService notificationService, ILoggerService<BookingService> logger,
        IMapper mapper)
    {
        _bookingRepository = bookingRepository;
        _placeRepository = placeRepository;
        _userRepository = userRepository;
        _notificationService = notificationService;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<BookingModel> GetBookingModelAsync(Guid id)
    {
        var booking = await GetBookingEntityAsync(id);

        return _mapper.Map<BookingModel>(booking);
    }

    public async Task<bool> IsPlaceAvailableAsync(Guid placeId, DateTime checkIn, DateTime checkOut, Guid? exceptBookingId = null)
    {
        var place = await _placeRepository.GetByIdAsync(placeId) ?? throw new NotFoundException("Place", placeId);

        return place.IsAvailable
               && !await _bookingRepository.HasOverlappingBookingAsync(placeId, checkIn, checkOut, exceptBookingId);
    }

    public async Task<BookingModel> CreateBookingAsync(BookingModel booking)
    {
        if (!await _userRepository.ExistsAsync(booking.UserId))
        {
            throw new NotFoundException("User", booking.UserId);
        }

        EnsureValidDates(booking.CheckInDate, booking.CheckOutDate, requireFuture: true);

        var place = await _placeRepository.GetByIdAsync(booking.PropertyId)
                    ?? throw new NotFoundException("Place", booking.PropertyId);

        if (booking.Adults + booking.Children > place.Capacity)
        {
            throw new BadRequestException($"Place {place.Id} accommodates up to {place.Capacity} guests");
        }

        if (!await IsPlaceAvailableAsync(place.Id, booking.CheckInDate, booking.CheckOutDate))
        {
            throw new ConflictException($"Place {place.Id} is not available for the selected dates");
        }

        var entity = _mapper.Map<BookingEntity>(booking);
        entity.Status = BookingStatus.Pending;
        entity.IsPaid = false;
        entity.IsFrozen = false;
        entity.Period = new Period { Start = booking.CheckInDate, End = booking.CheckOutDate };
        entity.TotalPrice = CalculateStayPrice(place.PricePerHour, booking.CheckInDate, booking.CheckOutDate);

        await _bookingRepository.AddAsync(entity);
        _logger.Log(ErrorLevel.Low, $"Booking {entity.Id} of place {place.Id} was created");

        await _notificationService.AddNotificationAsync(entity.UserId, NotificationType.Booking,
            "Booking created", $"{place.Name}: {entity.CheckInDate:d} - {entity.CheckOutDate:d}, {entity.TotalPrice} {entity.Currency}");

        return _mapper.Map<BookingModel>(entity);
    }

    public async Task<BookingModel> ChangeDatesAsync(Guid bookingId, PeriodModel period, int? adults = null, int? children = null)
    {
        var entity = await GetBookingEntityAsync(bookingId);
        EnsureChangeable(entity);
        EnsureValidDates(period.Start, period.End, requireFuture: true);

        if (entity.IsPaid)
        {
            throw new ConflictException($"Booking {bookingId} is paid, its dates cannot be changed");
        }

        var guests = (adults ?? entity.Adults) + (children ?? entity.Children);
        if (entity.Property != null && guests > entity.Property.Capacity)
        {
            throw new BadRequestException($"Place {entity.Property.Id} accommodates up to {entity.Property.Capacity} guests");
        }

        if (entity.PropertyId.HasValue
            && !await IsPlaceAvailableAsync(entity.PropertyId.Value, period.Start, period.End, entity.Id))
        {
            throw new ConflictException($"Place {entity.PropertyId} is not available for the selected dates");
        }

        entity.CheckInDate = period.Start;
        entity.CheckOutDate = period.End;
        entity.Period = new Period { Start = period.Start, End = period.End };
        entity.Adults = adults ?? entity.Adults;
        entity.Children = children ?? entity.Children;

        if (entity.Property != null)
        {
            entity.TotalPrice = CalculateStayPrice(entity.Property.PricePerHour, period.Start, period.End);
        }

        await _bookingRepository.UpdateAsync(entity);

        return _mapper.Map<BookingModel>(entity);
    }

    public async Task CancelAsync(Guid bookingId)
    {
        var entity = await GetBookingEntityAsync(bookingId);

        if (entity.Status == BookingStatus.Cancelled)
        {
            return;
        }

        EnsureChangeable(entity);

        if (entity.IsPaid)
        {
            throw new ConflictException($"Booking {bookingId} is paid and cannot be cancelled online");
        }

        entity.Status = BookingStatus.Cancelled;
        await _bookingRepository.UpdateAsync(entity);
        _logger.Log(ErrorLevel.Low, $"Booking {bookingId} was cancelled");
    }

    public async Task<BookingModel> PayAsync(Guid bookingId, string paymentMethod)
    {
        if (string.IsNullOrWhiteSpace(paymentMethod))
        {
            throw new BadRequestException("Payment method is required");
        }

        var entity = await GetBookingEntityAsync(bookingId);

        if (entity.IsPaid)
        {
            throw new ConflictException($"Booking {bookingId} has already been paid");
        }

        if (entity.Status is BookingStatus.Cancelled or BookingStatus.Completed)
        {
            throw new ConflictException($"Booking {bookingId} is {entity.Status} and cannot be paid");
        }

        // only the method name is stored, card data is processed by a payment provider
        entity.IsPaid = true;
        entity.PaymentMethod = paymentMethod.Trim();
        entity.Status = BookingStatus.Confirmed;
        await _bookingRepository.UpdateAsync(entity);

        await _notificationService.AddNotificationAsync(entity.UserId, NotificationType.Booking,
            "Booking confirmed", $"Booking {entity.Id} was paid ({entity.TotalPrice} {entity.Currency}).");

        return _mapper.Map<BookingModel>(entity);
    }

    /// <summary>
    /// Place rate is per hour, the booking is charged for the whole stay.
    /// </summary>
    public static decimal CalculateStayPrice(decimal pricePerHour, DateTime checkIn, DateTime checkOut) =>
        Math.Round(pricePerHour * (decimal)(checkOut - checkIn).TotalHours, 2);

    private static void EnsureValidDates(DateTime checkIn, DateTime checkOut, bool requireFuture)
    {
        if (checkIn >= checkOut)
        {
            throw new BadRequestException("Check-in date must be before check-out date");
        }

        if (requireFuture && checkIn.Date < DateTime.UtcNow.Date)
        {
            throw new BadRequestException("Check-in date cannot be in the past");
        }
    }

    private static void EnsureChangeable(BookingEntity entity)
    {
        if (entity.IsFrozen)
        {
            throw new ConflictException($"Booking {entity.Id} is frozen and cannot be changed");
        }

        if (entity.Status is BookingStatus.Cancelled or BookingStatus.Completed)
        {
            throw new ConflictException($"Booking {entity.Id} is {entity.Status} and cannot be changed");
        }
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
