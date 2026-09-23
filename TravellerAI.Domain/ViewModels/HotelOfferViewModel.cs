using TravellerAI.Domain.Enums;

namespace TravellerAI.Domain.ViewModels;

public class HotelOfferViewModel
{
    public Guid PlaceId { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public LocationViewModel Location { get; set; }
    public int Capacity { get; set; }
    public double AverageRating { get; set; }
    public FoodOptions Food { get; set; }
    public decimal PricePerHour { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int Nights { get; set; }
    public decimal TotalPrice { get; set; }
}
