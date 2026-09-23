namespace TravellerAI.Domain.ViewModels.Requests;

public class TripSearchRequest
{
    public Guid CountryId { get; set; }
    public string? City { get; set; }
    public Guid? StartLocationId { get; set; }
    public Guid? EndLocationId { get; set; }
}
