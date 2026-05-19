namespace TravellerAI.Domain.Models;

public class LocationModel
{
    public Guid Id { get; set; }
    string Country { get; set; }
    string City { get; set; }
    string Street { get; set; }
    string ZipCode { get; set; }
}