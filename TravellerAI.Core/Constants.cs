namespace TravellerAI.Core;

public static class Constants
{
    /// <summary>
    /// Limits used by command validators.
    /// </summary>
    public static class Validation
    {
        public const int MaxNameLength = 100;
        public const int MaxTitleLength = 200;
        public const int MaxLocationNameLength = 100;
        public const int MaxEmailLength = 256;
        public const int MaxTextLength = 500;

        public const int MinRating = 0;
        public const int MaxRating = 10;

        public const int MinAdults = 1;
        public const int MinChildren = 0;
        public const int MinSeatCount = 1;
        public const int MinBudget = 0;
        public const decimal MinPrice = 0;

        public const int MinUserAge = 13;
        public const int MaxUserAge = 120;
        public const int LanguageCodeLength = 2;
        /// <summary>Max number of chosen activities / trips / countries in a profile.</summary>
        public const int MaxProfileSelections = 20;

        /// <summary>Booking has to start at least this number of days from now.</summary>
        public const int MinDaysBeforeBooking = 1;
    }

    /// <summary>
    /// Journey setup and trip route rules.
    /// </summary>
    public static class Journey
    {
        public const int MaxJourneyDays = 30;
        public const int MaxMembers = 20;
        /// <summary>Hotel switch trip: old hotel -> new hotel.</summary>
        public const int MinTripStops = 2;
        public const int MaxTripStops = 12;
        /// <summary>Max distance between two consecutive stops of a day trip, km.</summary>
        public const double MaxStopDistanceKm = 30;
        /// <summary>Max total distance of a day trip, km.</summary>
        public const double MaxTripDistanceKm = 80;
    }

    /// <summary>
    /// Password and lockout policy (ASP.NET Core Identity options and validators).
    /// </summary>
    public static class Security
    {
        public const int MinPasswordLength = 8;
        public const int MaxPasswordLength = 128;
        public const int MaxFailedAccessAttempts = 5;
        public const int LockoutMinutes = 15;

        public const string UserRole = "User";
        public const string AdminRole = "Admin";
    }
}
