namespace TravellerAI.Domain.ViewModels.Requests;

public class LocationSearchRequest
{
    public string Country { get; set; }
    /// <summary>Optional - all locations of the country are returned without it.</summary>
    public string? City { get; set; }
}
