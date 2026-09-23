namespace TravellerAI.Domain.ViewModels;

public class TripViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Rating { get; set; }
    public bool IsPublic { get; set; }
    public Guid CountryId { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public double DistanceKm { get; set; }
    public Guid AuthorId { get; set; }
    public List<TripStopViewModel> Stops { get; set; }
}

public class TripStopViewModel
{
    public int Order { get; set; }
    public Guid LocationId { get; set; }
    public string? LocationName { get; set; }
    public string LocationStreet { get; set; }
    public double? LocationLatitude { get; set; }
    public double? LocationLongitude { get; set; }
    public Guid? ActivityId { get; set; }
    public string? ActivityName { get; set; }
    public decimal? ActivityPrice { get; set; }
    public double DistanceFromPreviousKm { get; set; }
}
