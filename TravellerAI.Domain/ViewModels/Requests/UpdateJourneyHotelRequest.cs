namespace TravellerAI.Domain.ViewModels.Requests;

public class UpdateJourneyHotelRequest
{
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int Adults { get; set; }
    public int Children { get; set; }
}
