using AutoMapper;
using TravellerAI.Core.Exceptions;
using TravellerAI.Core.Interfaces;
using TravellerAI.Core.Journeys;
using TravellerAI.Core.Repositories;
using TravellerAI.Domain.Entities;
using TravellerAI.Domain.Entities.Owned;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;
using static TravellerAI.Core.Constants;

namespace TravellerAI.Core.Services;

public class JourneyService : IJourneyService
{
    private readonly IJourneyRepository _journeyRepository;
    private readonly ITripRepository _tripRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRepository<PlaceEntity> _placeRepository;
    private readonly IRepository<CountryEntity> _countryRepository;
    private readonly IBookingService _bookingService;
    private readonly IBudgetService _budgetService;
    private readonly INotificationService _notificationService;
    private readonly ILoggerService<JourneyService> _logger;
    private readonly IMapper _mapper;

    public JourneyService(IJourneyRepository journeyRepository, ITripRepository tripRepository, IUserRepository userRepository,
        IRepository<PlaceEntity> placeRepository, IRepository<CountryEntity> countryRepository, IBookingService bookingService,
        IBudgetService budgetService, INotificationService notificationService, ILoggerService<JourneyService> logger, IMapper mapper)
    {
        _journeyRepository = journeyRepository;
        _tripRepository = tripRepository;
        _userRepository = userRepository;
        _placeRepository = placeRepository;
        _countryRepository = countryRepository;
        _bookingService = bookingService;
        _budgetService = budgetService;
        _notificationService = notificationService;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Guid> CreateJourneyAsync(Guid userId, string title, string? description, IEnumerable<string>? members)
    {
        if (!await _userRepository.ExistsAsync(userId))
        {
            throw new NotFoundException("User", userId);
        }

        var journey = new JourneyEntity
        {
            UserId = userId,
            Title = title.Trim(),
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
            Status = JourneyStatus.Active,
            Approved = false,
            Members = NormalizeMembers(members)
        };

        await _journeyRepository.AddAsync(journey);
        _logger.Log(ErrorLevel.Low, $"Journey {journey.Id} was created for user {userId}");

        return journey.Id;
    }

    public async Task<IReadOnlyList<JourneyModel>> GetUserJourneysAsync(Guid userId, bool? approved = null)
    {
        var journeys = await _journeyRepository.FindAsync(j => j.UserId == userId && (approved == null || j.Approved == approved));

        return journeys.OrderByDescending(j => j.Created).Select(ToModel).ToList();
    }

    public async Task<JourneyModel> GetJourneyAsync(Guid userId, Guid journeyId)
    {
        return ToModel(await GetOwnJourneyAsync(journeyId, userId));
    }

    public async Task DeleteJourneyAsync(Guid userId, Guid journeyId)
    {
        var journey = await GetEditableJourneyAsync(journeyId, userId);
        var stays = JourneyPlan.ActiveStays(journey);

        if (stays.Any(s => s.IsPaid))
        {
            throw new ConflictException("Journey has paid hotel bookings and cannot be deleted");
        }

        foreach (var stay in stays)
        {
            await _bookingService.CancelAsync(stay.Id);
        }

        await _journeyRepository.DeleteAsync(journeyId);
        _logger.Log(ErrorLevel.Low, $"Journey {journeyId} was deleted");
    }

    public async Task<JourneyProgressModel> GetProgressAsync(Guid userId, Guid journeyId)
    {
        var journey = await GetOwnJourneyAsync(journeyId, userId);

        return JourneyPlan.Evaluate(journey, DateTime.UtcNow);
    }

    public async Task<JourneyModel> SetPeriodAsync(Guid userId, Guid journeyId, DateTime start, DateTime end)
    {
        var journey = await GetEditableJourneyAsync(journeyId, userId);
        start = start.Date;
        end = end.Date;

        if (end <= start)
        {
            throw new BadRequestException("Journey has to end after it starts (at least one night)");
        }

        if ((end - start).Days + 1 > Journey.MaxJourneyDays)
        {
            throw new BadRequestException($"Journey cannot be longer than {Journey.MaxJourneyDays} days");
        }

        if (start < DateTime.UtcNow.Date)
        {
            throw new BadRequestException("Journey cannot start in the past");
        }

        journey.Period = new Period { Start = start, End = end };

        // days follow the period, trips of the remaining days are kept
        var dates = JourneyPlan.Dates(start, end).ToHashSet();
        foreach (var day in journey.Days.Where(d => !dates.Contains(d.Date.Date)).ToList())
        {
            journey.Days.Remove(day);
        }

        foreach (var date in dates.Except(journey.Days.Select(d => d.Date.Date)))
        {
            journey.Days.Add(new JourneyDayEntity { JourneyId = journey.Id, Date = date });
        }

        await _journeyRepository.UpdateAsync(journey);

        return ToModel(journey);
    }

    public async Task<JourneyModel> SelectCountryAsync(Guid userId, Guid journeyId, Guid countryId)
    {
        var journey = await GetEditableJourneyAsync(journeyId, userId);

        if (!await _countryRepository.ExistsAsync(countryId))
        {
            throw new NotFoundException("Country", countryId);
        }

        if (journey.CountryId == countryId)
        {
            return ToModel(journey);
        }

        if (JourneyPlan.ActiveStays(journey).Count > 0 || journey.Days.Any(d => d.TripId != null))
        {
            throw new ConflictException("Remove hotels and day trips of the journey before changing the country");
        }

        journey.CountryId = countryId;
        await _journeyRepository.UpdateAsync(journey);

        return ToModel(journey);
    }

    public async Task<JourneyModel> SetMembersAsync(Guid userId, Guid journeyId, IEnumerable<string> members)
    {
        var journey = await GetEditableJourneyAsync(journeyId, userId);

        journey.Members = NormalizeMembers(members);
        await _journeyRepository.UpdateAsync(journey);

        return ToModel(journey);
    }

    public async Task<JourneyModel> SetBudgetAsync(Guid userId, Guid journeyId, decimal budget)
    {
        var journey = await GetEditableJourneyAsync(journeyId, userId);

        await _budgetService.SetJourneyBudgetAsync(journeyId, budget);

        return ToModel(journey);
    }

    public async Task<IReadOnlyList<HotelOfferModel>> SearchHotelsAsync(Guid userId, Guid journeyId, DateTime? checkIn,
        DateTime? checkOut, string? city, int guests)
    {
        var journey = await GetOwnJourneyAsync(journeyId, userId);
        var (from, to) = GetStayDates(journey, checkIn ?? journey.Period?.Start, checkOut ?? journey.Period?.End);
        city = string.IsNullOrWhiteSpace(city) ? null : city.Trim();
        var countryId = journey.CountryId;

        var places = await _placeRepository.FindAsync(p =>
            p.IsAvailable
            && p.Location != null && p.Location.CountryId == countryId
            && p.Location.Latitude != null && p.Location.Longitude != null
            && (city == null || p.Location.City == city)
            && p.Capacity >= guests);

        var offers = new List<HotelOfferModel>();
        foreach (var place in places)
        {
            if (!await _bookingService.IsPlaceAvailableAsync(place.Id, from, to))
            {
                continue;
            }

            offers.Add(new HotelOfferModel
            {
                PlaceId = place.Id,
                Name = place.Name,
                Type = place.Type,
                Location = _mapper.Map<LocationModel>(place.Location),
                Capacity = place.Capacity,
                AverageRating = place.AverageRating,
                Food = place.Food,
                PricePerHour = place.PricePerHour,
                CheckIn = from,
                CheckOut = to,
                Nights = (to - from).Days,
                TotalPrice = BookingService.CalculateStayPrice(place.PricePerHour, from, to)
            });
        }

        return offers.OrderBy(o => o.TotalPrice).ToList();
    }

    public async Task<JourneyModel> AddHotelAsync(Guid userId, Guid journeyId, Guid placeId, DateTime checkIn, DateTime checkOut,
        int adults, int children)
    {
        var journey = await GetEditableJourneyAsync(journeyId, userId);
        var (from, to) = GetStayDates(journey, checkIn, checkOut);

        var place = await _placeRepository.GetByIdAsync(placeId) ?? throw new NotFoundException("Place", placeId);
        EnsureHotelFits(journey, place);
        EnsureNoOverlap(journey, from, to, exceptBookingId: null);

        await _bookingService.CreateBookingAsync(new BookingModel
        {
            UserId = userId,
            JourneyId = journeyId,
            PropertyId = placeId,
            CheckInDate = from,
            CheckOutDate = to,
            Adults = adults,
            Children = children
        });

        return ToModel(journey);
    }

    public async Task<JourneyModel> UpdateHotelAsync(Guid userId, Guid journeyId, Guid bookingId, DateTime checkIn, DateTime checkOut,
        int adults, int children)
    {
        var journey = await GetEditableJourneyAsync(journeyId, userId);
        var stay = GetJourneyStay(journey, bookingId);
        var (from, to) = GetStayDates(journey, checkIn, checkOut);

        EnsureNoOverlap(journey, from, to, exceptBookingId: stay.Id);

        await _bookingService.ChangeDatesAsync(stay.Id, new PeriodModel { Start = from, End = to }, adults, children);

        return ToModel(journey);
    }

    public async Task<JourneyModel> RemoveHotelAsync(Guid userId, Guid journeyId, Guid bookingId)
    {
        var journey = await GetEditableJourneyAsync(journeyId, userId);
        var stay = GetJourneyStay(journey, bookingId);

        await _bookingService.CancelAsync(stay.Id);

        return ToModel(journey);
    }

    public async Task<IReadOnlyList<TripModel>> GetAvailableDayTripsAsync(Guid userId, Guid journeyId, DateTime date)
    {
        var journey = await GetOwnJourneyAsync(journeyId, userId);
        GetDay(journey, date);

        var (start, end) = JourneyPlan.DayHotels(JourneyPlan.ActiveStays(journey), date.Date);
        var startLocation = start?.Property?.LocationId;
        var endLocation = end?.Property?.LocationId;

        if (startLocation == null || endLocation == null)
        {
            throw new BadRequestException($"Select hotels for {date:yyyy-MM-dd} before choosing a trip");
        }

        var trips = await _tripRepository.SearchCatalogAsync(journey.CountryId!.Value, city: null, userId);

        return trips
            .Where(t => t.Stops.Count > 0)
            .Where(t => t.Stops.MinBy(s => s.Order)!.LocationId == startLocation && t.Stops.MaxBy(s => s.Order)!.LocationId == endLocation)
            .OrderBy(t => t.DistanceKm)
            .Select(t => _mapper.Map<TripModel>(t))
            .ToList();
    }

    public async Task<JourneyModel> AssignDayTripAsync(Guid userId, Guid journeyId, DateTime date, Guid tripId)
    {
        var journey = await GetEditableJourneyAsync(journeyId, userId);
        var day = GetDay(journey, date);

        var trip = await _tripRepository.GetByIdAsync(tripId) ?? throw new NotFoundException("Trip", tripId);
        if (!trip.IsPublic && trip.UserId != userId)
        {
            throw new ForbiddenException($"Trip {tripId} is private");
        }

        var errors = JourneyPlan.ValidateDayTrip(journey, day.Date, trip);
        if (errors.Count > 0)
        {
            throw new BadRequestException(string.Join(" ", errors));
        }

        day.TripId = trip.Id;
        day.Trip = trip;
        await _journeyRepository.UpdateAsync(journey);

        return ToModel(journey);
    }

    public async Task<JourneyModel> ClearDayTripAsync(Guid userId, Guid journeyId, DateTime date)
    {
        var journey = await GetEditableJourneyAsync(journeyId, userId);
        var day = GetDay(journey, date);

        day.TripId = null;
        day.Trip = null;
        await _journeyRepository.UpdateAsync(journey);

        return ToModel(journey);
    }

    public async Task<JourneyModel> ApproveAsync(Guid userId, Guid journeyId)
    {
        var journey = await GetOwnJourneyAsync(journeyId, userId);
        EnsureNotCancelled(journey);

        if (journey.Approved)
        {
            throw new ConflictException($"Journey {journeyId} is already approved");
        }

        var progress = JourneyPlan.Evaluate(journey, DateTime.UtcNow);
        if (!progress.CanApprove)
        {
            var errors = progress.Steps.SelectMany(s => s.Errors.Select(e => $"{s.Step}: {e}"));
            throw new BadRequestException($"Journey setup is not complete. {string.Join(" ", errors)}");
        }

        var stays = JourneyPlan.ActiveStays(journey);
        foreach (var stay in stays)
        {
            // bookings of other users could appear since the stay was selected
            if (!await _bookingService.IsPlaceAvailableAsync(stay.PropertyId!.Value, stay.CheckInDate, stay.CheckOutDate, stay.Id))
            {
                throw new ConflictException($"Hotel '{stay.Property?.Name}' is not available anymore, select another hotel");
            }
        }

        journey.Approved = true;
        journey.ApprovedAt = DateTime.UtcNow;
        foreach (var stay in stays)
        {
            stay.IsFrozen = true;
        }

        await _journeyRepository.UpdateAsync(journey);
        await _budgetService.RecalculateJourneyBudgetAsync(journeyId);

        await _notificationService.AddNotificationAsync(userId, NotificationType.Journey, "Journey approved",
            $"'{journey.Title}' setup is finished: {journey.Period!.Start:d} - {journey.Period.End:d}.");
        _logger.Log(ErrorLevel.Low, $"Journey {journeyId} was approved");

        return ToModel(journey);
    }

    public async Task<JourneyModel> UnapproveAsync(Guid userId, Guid journeyId)
    {
        var journey = await GetOwnJourneyAsync(journeyId, userId);
        EnsureNotCancelled(journey);

        if (!journey.Approved)
        {
            throw new ConflictException($"Journey {journeyId} is not approved");
        }

        if (journey.Period != null && journey.Period.Start.Date <= DateTime.UtcNow.Date)
        {
            throw new ConflictException("Journey has already started and cannot be edited");
        }

        journey.Approved = false;
        journey.ApprovedAt = null;
        foreach (var stay in JourneyPlan.ActiveStays(journey))
        {
            stay.IsFrozen = false;
        }

        await _journeyRepository.UpdateAsync(journey);

        await _notificationService.AddNotificationAsync(userId, NotificationType.Journey, "Journey editing",
            $"'{journey.Title}' is a draft again. Approve it when the changes are done.");
        _logger.Log(ErrorLevel.Low, $"Journey {journeyId} was unapproved");

        return ToModel(journey);
    }

    public async Task<JourneyStatus> GetJourneyStatusAsync(Guid journeyId)
    {
        var journey = await GetJourneyEntityAsync(journeyId);

        return journey.Status;
    }

    public async Task EnsureOwnerAsync(Guid journeyId, Guid userId)
    {
        await GetOwnJourneyAsync(journeyId, userId);
    }

    public async Task EnsureEditableAsync(Guid journeyId, Guid userId)
    {
        await GetEditableJourneyAsync(journeyId, userId);
    }

    private JourneyModel ToModel(JourneyEntity journey)
    {
        var model = _mapper.Map<JourneyModel>(journey);
        var stays = JourneyPlan.ActiveStays(journey);

        foreach (var day in model.Days)
        {
            var (start, end) = JourneyPlan.DayHotels(stays, day.Date);
            day.StartLocationId = start?.Property?.LocationId;
            day.EndLocationId = end?.Property?.LocationId;
            day.IsHotelSwitch = JourneyPlan.IsHotelSwitch(start, end);
        }

        return model;
    }

    private static List<string> NormalizeMembers(IEnumerable<string>? members)
    {
        var result = (members ?? Enumerable.Empty<string>())
            .Where(m => !string.IsNullOrWhiteSpace(m))
            .Select(m => m.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (result.Count > Journey.MaxMembers)
        {
            throw new BadRequestException($"Journey can have up to {Journey.MaxMembers} members");
        }

        return result;
    }

    /// <summary>
    /// Stay dates inside the journey period; the journey period and the country have to be set.
    /// </summary>
    private static (DateTime From, DateTime To) GetStayDates(JourneyEntity journey, DateTime? checkIn, DateTime? checkOut)
    {
        if (journey.Period == null || journey.CountryId == null)
        {
            throw new BadRequestException("Set the journey period and the country before selecting hotels");
        }

        var from = (checkIn ?? journey.Period.Start).Date;
        var to = (checkOut ?? journey.Period.End).Date;

        if (to <= from)
        {
            throw new BadRequestException("Check-out has to be after check-in");
        }

        if (from < journey.Period.Start.Date || to > journey.Period.End.Date)
        {
            throw new BadRequestException(
                $"Stay has to be inside the journey period {journey.Period.Start:yyyy-MM-dd} - {journey.Period.End:yyyy-MM-dd}");
        }

        return (from, to);
    }

    private static void EnsureHotelFits(JourneyEntity journey, PlaceEntity place)
    {
        if (place.Location == null || place.Location.CountryId != journey.CountryId)
        {
            throw new BadRequestException($"Hotel '{place.Name}' is not in the journey country");
        }

        // the hotel is the first and the last stop of day trips
        if (place.Location.Latitude == null || place.Location.Longitude == null)
        {
            throw new BadRequestException($"Hotel '{place.Name}' has no coordinates");
        }
    }

    private static void EnsureNoOverlap(JourneyEntity journey, DateTime from, DateTime to, Guid? exceptBookingId)
    {
        var overlapping = JourneyPlan.ActiveStays(journey)
            .FirstOrDefault(s => s.Id != exceptBookingId && s.CheckInDate.Date < to && from < s.CheckOutDate.Date);

        if (overlapping != null)
        {
            throw new ConflictException(
                $"Stay overlaps with hotel '{overlapping.Property?.Name}' ({overlapping.CheckInDate:yyyy-MM-dd} - {overlapping.CheckOutDate:yyyy-MM-dd})");
        }
    }

    private static BookingEntity GetJourneyStay(JourneyEntity journey, Guid bookingId) =>
        JourneyPlan.ActiveStays(journey).FirstOrDefault(s => s.Id == bookingId)
        ?? throw new NotFoundException($"Hotel booking {bookingId} of journey {journey.Id} not found");

    private static JourneyDayEntity GetDay(JourneyEntity journey, DateTime date)
    {
        if (journey.Period == null)
        {
            throw new BadRequestException("Set the journey period first");
        }

        return journey.Days.FirstOrDefault(d => d.Date.Date == date.Date)
               ?? throw new BadRequestException($"{date:yyyy-MM-dd} is outside the journey period");
    }

    private static void EnsureNotCancelled(JourneyEntity journey)
    {
        if (journey.Status == JourneyStatus.Cancelled)
        {
            throw new ConflictException($"Journey {journey.Id} is cancelled");
        }
    }

    private async Task<JourneyEntity> GetJourneyEntityAsync(Guid journeyId)
    {
        var journey = await _journeyRepository.GetByIdAsync(journeyId);

        if (journey == null)
        {
            _logger.Log(ErrorLevel.Medium, $"Journey {journeyId} not found");
            throw new NotFoundException("Journey", journeyId);
        }

        return journey;
    }

    private async Task<JourneyEntity> GetOwnJourneyAsync(Guid journeyId, Guid userId)
    {
        var journey = await GetJourneyEntityAsync(journeyId);

        if (journey.UserId != userId)
        {
            throw new ForbiddenException($"Journey {journeyId} does not belong to user {userId}");
        }

        return journey;
    }

    /// <summary>
    /// Approved journeys are read-only until they are unapproved.
    /// </summary>
    private async Task<JourneyEntity> GetEditableJourneyAsync(Guid journeyId, Guid userId)
    {
        var journey = await GetOwnJourneyAsync(journeyId, userId);
        EnsureNotCancelled(journey);

        if (journey.Approved)
        {
            throw new ConflictException($"Journey {journeyId} is approved. Unapprove it to continue editing");
        }

        return journey;
    }
}
