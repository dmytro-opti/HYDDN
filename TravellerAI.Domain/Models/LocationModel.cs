namespace TravellerAI.Domain.Models;

public class LocationModel
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public Guid CountryId { get; set; }
    /// <summary>Country name.</summary>
    public string Country { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string ZipCode { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}
