using TravellerAI.Domain.Entities;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Journeys;

/// <summary>
/// Journey setup rules, shared by the progress endpoint, day trip assignment and approval.
/// Hotel stays cover nights [CheckIn, CheckOut). On a day the trip starts at the hotel of the previous night
/// and finishes at the hotel of the coming night (arrival / departure days use the only available hotel),
/// so on a hotel switch day it goes from the old hotel to the new one.
/// </summary>
public static class JourneyPlan
{
    public static IReadOnlyList<BookingEntity> ActiveStays(JourneyEntity journey) =>
        journey.Bookings
            .Where(b => b.Status != BookingStatus.Cancelled)
            .OrderBy(b => b.CheckInDate)
            .ToList();

    public static IEnumerable<DateTime> Dates(DateTime start, DateTime end)
    {
        for (var date = start.Date; date <= end.Date; date = date.AddDays(1))
        {
            yield return date;
        }
    }

    public static BookingEntity? NightStay(IReadOnlyList<BookingEntity> stays, DateTime night) =>
        stays.FirstOrDefault(s => s.CheckInDate.Date <= night.Date && night.Date < s.CheckOutDate.Date);

    /// <summary>
    /// Hotels the day trip starts from and finishes at.
    /// </summary>
    public static (BookingEntity? Start, BookingEntity? End) DayHotels(IReadOnlyList<BookingEntity> stays, DateTime date)
    {
        var previousNight = NightStay(stays, date.AddDays(-1));
        var comingNight = NightStay(stays, date);

        return (previousNight ?? comingNight, comingNight ?? previousNight);
    }

    public static bool IsHotelSwitch(BookingEntity? start, BookingEntity? end) =>
        start != null && end != null && start.PropertyId != end.PropertyId;

    /// <summary>
    /// Errors of scheduling the trip on the date; empty when the trip fits.
    /// </summary>
    public static List<string> ValidateDayTrip(JourneyEntity journey, DateTime date, TripEntity trip)
    {
        var errors = new List<string>();
        var day = $"{date:yyyy-MM-dd}";

        if (trip.CountryId != journey.CountryId)
        {
            errors.Add($"{day}: trip '{trip.Name}' is not in the journey country");
            return errors;
        }

        var (start, end) = DayHotels(ActiveStays(journey), date);
        if (start?.Property?.LocationId == null || end?.Property?.LocationId == null)
        {
            errors.Add($"{day}: select hotels for this day before planning a trip");
            return errors;
        }

        var stops = trip.Stops.OrderBy(s => s.Order).ToList();
        if (stops.Count == 0)
        {
            errors.Add($"{day}: trip '{trip.Name}' has no stops");
            return errors;
        }

        if (stops[0].LocationId != start.Property.LocationId)
        {
            errors.Add($"{day}: trip '{trip.Name}' has to start at hotel '{start.Property.Name}'");
        }

        if (stops[^1].LocationId != end.Property.LocationId)
        {
            errors.Add($"{day}: trip '{trip.Name}' has to finish at hotel '{end.Property.Name}'");
        }

        return errors;
    }

    public static JourneyProgressModel Evaluate(JourneyEntity journey, DateTime today)
    {
        var details = new JourneyStepModel { Step = JourneyStep.Details };
        var period = new JourneyStepModel { Step = JourneyStep.Period };
        var country = new JourneyStepModel { Step = JourneyStep.Country };
        var hotels = new JourneyStepModel { Step = JourneyStep.Hotels };
        var trips = new JourneyStepModel { Step = JourneyStep.Trips };

        if (string.IsNullOrWhiteSpace(journey.Title))
        {
            details.Errors.Add("Title is empty");
        }

        var hasPeriod = journey.Period != null;
        if (!hasPeriod)
        {
            period.Errors.Add("Period is not set");
        }
        else
        {
            var days = (journey.Period!.End.Date - journey.Period.Start.Date).Days + 1;
            if (days < 2)
            {
                period.Errors.Add("Journey has to include at least one night");
            }

            if (days > Constants.Journey.MaxJourneyDays)
            {
                period.Errors.Add($"Journey cannot be longer than {Constants.Journey.MaxJourneyDays} days");
            }

            if (!journey.Approved && journey.Period.Start.Date < today.Date)
            {
                period.Errors.Add("Journey start date is in the past");
            }
        }

        if (journey.CountryId == null)
        {
            country.Errors.Add("Country is not selected");
        }

        var stays = ActiveStays(journey);
        if (!hasPeriod || journey.CountryId == null)
        {
            hotels.Errors.Add("Set the period and the country first");
        }
        else
        {
            EvaluateHotels(journey, stays, hotels.Errors);
        }

        if (hotels.Errors.Count > 0)
        {
            trips.Errors.Add("Select hotels for every night first");
        }
        else
        {
            EvaluateTrips(journey, stays, trips.Errors);
        }

        return new JourneyProgressModel
        {
            JourneyId = journey.Id,
            Approved = journey.Approved,
            Steps = new List<JourneyStepModel> { details, period, country, hotels, trips }
        };
    }

    private static void EvaluateHotels(JourneyEntity journey, IReadOnlyList<BookingEntity> stays, List<string> errors)
    {
        if (stays.Count == 0)
        {
            errors.Add("No hotel is selected");
            return;
        }

        var start = journey.Period!.Start.Date;
        var end = journey.Period.End.Date;

        foreach (var stay in stays)
        {
            var name = stay.Property?.Name ?? stay.Id.ToString();

            if (stay.Property?.Location == null)
            {
                errors.Add($"Hotel '{name}' has no location");
            }
            else if (stay.Property.Location.CountryId != journey.CountryId)
            {
                errors.Add($"Hotel '{name}' is not in the journey country");
            }

            if (stay.CheckInDate.Date < start || stay.CheckOutDate.Date > end)
            {
                errors.Add($"Stay at '{name}' is outside the journey period");
            }
        }

        for (var night = start; night < end; night = night.AddDays(1))
        {
            var count = stays.Count(s => s.CheckInDate.Date <= night && night < s.CheckOutDate.Date);
            if (count == 0)
            {
                errors.Add($"Night {night:yyyy-MM-dd} has no hotel");
            }
            else if (count > 1)
            {
                errors.Add($"Night {night:yyyy-MM-dd} has several hotels");
            }
        }
    }

    private static void EvaluateTrips(JourneyEntity journey, IReadOnlyList<BookingEntity> stays, List<string> errors)
    {
        var days = journey.Days.OrderBy(d => d.Date).ToList();
        var dates = Dates(journey.Period!.Start, journey.Period.End).ToList();

        if (days.Count != dates.Count || days.Select(d => d.Date.Date).Except(dates).Any())
        {
            errors.Add("Journey days do not match the period, set the period again");
            return;
        }

        foreach (var day in days)
        {
            var (start, end) = DayHotels(stays, day.Date);

            if (day.Trip == null)
            {
                if (IsHotelSwitch(start, end))
                {
                    errors.Add($"{day.Date:yyyy-MM-dd}: hotel switch day needs a trip from '{start!.Property?.Name}' to '{end!.Property?.Name}'");
                }

                continue;
            }

            errors.AddRange(ValidateDayTrip(journey, day.Date, day.Trip));
        }

        if (days.All(d => d.TripId == null))
        {
            errors.Add("No trips are planned");
        }
    }
}
