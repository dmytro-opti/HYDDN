namespace TravellerAI.Domain.Entities;

public class LocationEntity : BaseEntity
{
    /// <summary>Optional name of the point, e.g. "Lviv Opera House".</summary>
    public string? Name { get; set; }

    public Guid CountryId { get; set; }
    public virtual CountryEntity Country { get; set; } = null!;

    public string? City { get; set; }
    public string? Street { get; set; }
    public string? ZipCode { get; set; }

    /// <summary>WGS 84 coordinates, used for routes and distances.</summary>
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}
