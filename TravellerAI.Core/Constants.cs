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

        public const int MinRating = 0;
        public const int MaxRating = 10;

        public const int MinAdults = 1;
        public const int MinChildren = 0;
        public const int MinSeatCount = 1;
        public const int MinAge = 1;
        public const int MinBudget = 0;
        public const decimal MinPrice = 0;

        /// <summary>Booking has to start at least this number of days from now.</summary>
        public const int MinDaysBeforeBooking = 1;
    }
}
