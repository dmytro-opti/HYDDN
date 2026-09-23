namespace TravellerAI.Domain.ViewModels.Requests;

public class PayBookingRequest
{
    /// <summary>Payment method name, e.g. "card". Card data is handled by a payment provider only.</summary>
    public string PaymentMethod { get; set; }
}
