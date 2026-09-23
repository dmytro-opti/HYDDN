namespace TravellerAI.Domain.ViewModels.Requests;

public class HotelSearchRequest
{
    /// <summary>Defaults to the journey start.</summary>
    public DateTime? CheckIn { get; set; }
    /// <summary>Defaults to the journey end.</summary>
    public DateTime? CheckOut { get; set; }
    public string? City { get; set; }
    public int Guests { get; set; } = 1;
}
