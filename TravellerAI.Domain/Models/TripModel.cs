namespace TravellerAI.Domain.Models;

public class TripModel
{
    public Guid Id { get; set; }
    public string Name  { get; set; }
    public UserModel User { get; set; }
    public GroupModel Group { get; set; }
    ICollection<JourneyModel>  Journeys { get; set; } = new List<JourneyModel>();
    public BudgetModel Budget { get; set; }
    public BookingModel Booking { get; set; }
    public DiscountModel Discount { get; set; }
    public MapModel Map { get; set; }
    public PeriodModel Period { get; set; }
    public double Rating { get; set; }
}