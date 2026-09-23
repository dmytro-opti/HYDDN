using TravellerAI.Domain.Enums;

namespace TravellerAI.Domain.Models;

/// <summary>
/// Place available for the requested stay with the calculated price.
/// </summary>
public class HotelOfferModel
{
    public Guid PlaceId { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public LocationModel Location { get; set; }
    public int Capacity { get; set; }
    public double AverageRating { get; set; }
    public FoodOptions Food { get; set; }
    public decimal PricePerHour { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int Nights { get; set; }
    public decimal TotalPrice { get; set; }
}
