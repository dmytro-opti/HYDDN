using AutoMapper;
using TravellerAI.Core.Exceptions;
using TravellerAI.Core.Features.BuildTripCommand;
using TravellerAI.Core.Interfaces;
using TravellerAI.Core.Repositories;
using TravellerAI.Domain.Entities;
using TravellerAI.Domain.Entities.Owned;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Exceptions;
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
            throw new ResourceNotFoundException($"User {userId} not found");
        }

        var trip = new TripEntity
        {
            Name = command.Name,
            UserId = userId,
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
            throw new ResourceNotFoundException($"Trip {tripId} not found");
        }

        _logger.Log(ErrorLevel.Low, $"Trip {tripId} was deleted");

        return tripId;
    }

    public async Task<Guid> AddPeriodTrip(BuildTripCommand command)
    {
        var trip = await GetTripEntityAsync(command.TripId);

        trip.Period = ToPeriod(command.Period);
        await _tripRepository.UpdateAsync(trip);

        return trip.Id;
    }

    public async Task SelectPeriod(TripModel trip, PeriodViewModel period)
    {
        var entity = await GetTripEntityAsync(trip.TripId);

        entity.Period = ToPeriod(period);
        await _tripRepository.UpdateAsync(entity);

        trip.Period = _mapper.Map<PeriodModel>(entity.Period);
    }

    /// <summary>
    /// Recalculates trip budget total from transports and booking.
    /// </summary>
    public async Task Build(TripModel trip)
    {
        var entity = await GetTripEntityAsync(trip.TripId);

        var total = entity.Transports.Sum(t => t.Price) + (entity.Booking?.TotalPrice ?? 0);

        entity.Budget ??= new BudgetEntity();
        entity.Budget.Total = total;
        await _tripRepository.UpdateAsync(entity);

        if (entity.Budget.Budget > 0 && total > entity.Budget.Budget)
        {
            _logger.Log(ErrorLevel.Medium, $"Trip {entity.Id} exceeds its budget: {total} > {entity.Budget.Budget}");
        }
    }

    public Task<TripModel> Show(TripModel trip)
    {
        return GetTripAsync(trip.TripId);
    }

    public async Task<bool> UpdateTripAsync(TripModel trip)
    {
        var entity = await GetTripEntityAsync(trip.TripId);

        _mapper.Map(trip, entity);

        if (trip.Booking != null)
        {
            UpsertBooking(entity, trip.Booking);
        }

        await _tripRepository.UpdateAsync(entity);
        _logger.Log(ErrorLevel.Low, $"Trip {entity.Id} was updated");

        return true;
    }

    private void UpsertBooking(TripEntity trip, BookingModel booking)
    {
        if (trip.Booking != null && trip.Booking.Id == booking.BookingId)
        {
            _mapper.Map(booking, trip.Booking);
            return;
        }

        // a new booking replaces the previous one, which is cancelled
        if (trip.Booking != null)
        {
            trip.Booking.Status = BookingStatus.Cancelled;
        }

        var newBooking = _mapper.Map<BookingEntity>(booking);
        newBooking.UserId = trip.UserId;
        newBooking.JourneyId ??= trip.JourneyId;
        trip.Booking = newBooking;
    }

    private async Task<TripEntity> GetTripEntityAsync(Guid tripId)
    {
        var trip = await _tripRepository.GetByIdAsync(tripId);

        if (trip == null)
        {
            _logger.Log(ErrorLevel.Medium, $"Trip {tripId} not found");
            throw new ResourceNotFoundException($"Trip {tripId} not found");
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
