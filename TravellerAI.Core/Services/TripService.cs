using AutoMapper;
using TravellerAI.Core.Exceptions;
using TravellerAI.Core.Features.BuildTripCommand;
using TravellerAI.Core.Interfaces;
using TravellerAI.Core.Repositories;
using TravellerAI.Domain.Entities;
using TravellerAI.Domain.Entities.Owned;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;
using TravellerAI.Domain.ViewModels;

namespace TravellerAI.Core.Services;

public class TripService : ITripService
{
    private readonly ITripRepository _tripRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILoggerService<TripService> _logger;
    private readonly IMapper _mapper;

    public TripService(ITripRepository tripRepository, IUserRepository userRepository,
        ILoggerService<TripService> logger, IMapper mapper)
    {
        _tripRepository = tripRepository;
        _userRepository = userRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Guid> CreateTrip(BuildTripCommand command)
    {
        var userId = command.User?.Id ?? throw new BadRequestException("Trip user is not specified");

        if (!await _userRepository.ExistsAsync(userId))
        {
            throw new NotFoundException("User", userId);
        }

        var trip = new TripEntity
        {
            Name = command.Name,
            UserId = userId,
            Status = TripStatus.Active,
            Period = command.Period == null ? null : ToPeriod(command.Period),
            Budget = command.Budget > 0 ? new BudgetEntity { Budget = command.Budget } : null
        };

        await _tripRepository.AddAsync(trip);
        _logger.Log(ErrorLevel.Low, $"Trip {trip.Id} was created for user {userId}");

        return trip.Id;
    }

    public async Task<TripModel> GetTripAsync(Guid tripId)
    {
        var trip = await GetTripEntityAsync(tripId);

        return _mapper.Map<TripModel>(trip);
    }

    public async Task<Guid> DeleteTrip(Guid tripId)
    {
        if (!await _tripRepository.DeleteAsync(tripId))
        {
            throw new NotFoundException("Trip", tripId);
        }

        _logger.Log(ErrorLevel.Low, $"Trip {tripId} was deleted");

        return tripId;
    }

    public async Task<Guid> AddPeriodTrip(BuildTripCommand command)
    {
        var trip = await GetActiveTripEntityAsync(command.TripId);

        trip.Period = ToPeriod(command.Period);
        await _tripRepository.UpdateAsync(trip);

        return trip.Id;
    }

    public async Task SelectPeriod(TripModel trip, PeriodViewModel period)
    {
        var entity = await GetActiveTripEntityAsync(trip.TripId);

        entity.Period = ToPeriod(period);
        await _tripRepository.UpdateAsync(entity);

        trip.Period = _mapper.Map<PeriodModel>(entity.Period);
    }

    /// <summary>
    /// Recalculates budget totals of the trip (and of its journey) from transports and bookings.
    /// </summary>
    public async Task Build(TripModel trip)
    {
        var entity = await GetActiveTripEntityAsync(trip.TripId);

        entity.Budget ??= new BudgetEntity();
        entity.Budget.Total = CalculateTripCost(entity);

        if (entity.Journey?.Budget != null)
        {
            entity.Journey.Budget.Total = entity.Journey.Trips.Sum(CalculateTripCost);
        }

        await _tripRepository.UpdateAsync(entity);

        if (entity.Budget.Budget > 0 && entity.Budget.Total > entity.Budget.Budget)
        {
            _logger.Log(ErrorLevel.Medium, $"Trip {entity.Id} exceeds its budget: {entity.Budget.Total} > {entity.Budget.Budget}");
        }
    }

    public Task<TripModel> Show(TripModel trip)
    {
        return GetTripAsync(trip.TripId);
    }

    public async Task<bool> UpdateTripAsync(TripModel trip)
    {
        var entity = await GetActiveTripEntityAsync(trip.TripId);

        _mapper.Map(trip, entity);

        if (trip.Booking != null)
        {
            UpsertBooking(entity, trip.Booking);
        }

        await _tripRepository.UpdateAsync(entity);
        _logger.Log(ErrorLevel.Low, $"Trip {entity.Id} was updated");

        return true;
    }

    public async Task<TripStatus> GetTripStatusAsync(Guid tripId)
    {
        var trip = await GetTripEntityAsync(tripId);

        return trip.Status;
    }

    private void UpsertBooking(TripEntity trip, BookingModel booking)
    {
        var current = trip.Booking;

        if (current is { IsFrozen: true })
        {
            throw new ConflictException($"Booking {current.Id} of trip {trip.Id} is frozen and cannot be changed");
        }

        if (current != null && current.Id == booking.BookingId)
        {
            _mapper.Map(booking, current);
            return;
        }

        // a new booking replaces the previous one, which is cancelled
        if (current != null)
        {
            current.Status = BookingStatus.Cancelled;
        }

        var newBooking = _mapper.Map<BookingEntity>(booking);
        newBooking.UserId = trip.UserId;
        newBooking.JourneyId ??= trip.JourneyId;
        trip.Booking = newBooking;
    }

    private static decimal CalculateTripCost(TripEntity trip)
    {
        var bookingCost = trip.Booking is { Status: not BookingStatus.Cancelled } booking ? booking.TotalPrice : 0;

        return trip.Transports.Sum(t => t.Price) + bookingCost;
    }

    private async Task<TripEntity> GetTripEntityAsync(Guid tripId)
    {
        var trip = await _tripRepository.GetByIdAsync(tripId);

        if (trip == null)
        {
            _logger.Log(ErrorLevel.Medium, $"Trip {tripId} not found");
            throw new NotFoundException("Trip", tripId);
        }

        return trip;
    }

    /// <summary>
    /// Only active trips can be changed.
    /// </summary>
    private async Task<TripEntity> GetActiveTripEntityAsync(Guid tripId)
    {
        var trip = await GetTripEntityAsync(tripId);

        if (trip.Status != TripStatus.Active)
        {
            throw new ConflictException($"Trip {tripId} is {trip.Status} and cannot be changed");
        }

        return trip;
    }

    private static Period ToPeriod(PeriodViewModel period)
    {
        if (period == null || period.Start >= period.End)
        {
            throw new BadRequestException("Period start must be before period end");
        }

        return new Period { Start = period.Start, End = period.End };
    }
}
