namespace TravellerAI.Domain.ViewModels;

public class LocationViewModel
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public Guid CountryId { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string ZipCode { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}
