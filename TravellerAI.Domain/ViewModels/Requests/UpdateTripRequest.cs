namespace TravellerAI.Domain.ViewModels.Requests;

public class UpdateTripRequest
{
    public string Name { get; set; }
    public PeriodViewModel Period { get; set; }
    public double Rating { get; set; }
    /// <summary>Optional. Same BookingId updates the current booking, otherwise a new booking replaces it.</summary>
    public TripBookingRequest? Booking { get; set; }
}
