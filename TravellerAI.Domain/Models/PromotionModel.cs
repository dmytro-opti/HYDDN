namespace TravellerAI.Domain.Models;

public class PromotionModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string PromoCode { get; set; }
    public int Discount { get; set; }
    public PeriodModel Period { get; set; }
    public bool IsActive { get; set; } = false;
    
}