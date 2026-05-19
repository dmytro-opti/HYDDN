using TravellerAI.Domain.Enums;
namespace TravellerAI.Domain.Models;
public class PlaceModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    public string Address { get; set; }
    public int LocationId { get; set; }
    public decimal PricePerHour { get; set; }
    public int Capacity { get; set; }
    public bool IsAvailable { get; set; }
    public PlaceStatus Status { get; set; }
    public double AverageRating { get; set; }
    public IEnumerable<ReviewModel> Reviews { get; set; }
    public IEnumerable<string> ImageUrls { get; set; }
    public FoodOptions Food { get; set; } 
    public string BookingRules { get; set; }
    public IEnumerable<BookingModel> Bookings { get; set; }
    public UserModel Owner { get; set; }
}