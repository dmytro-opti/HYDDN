namespace TravellerAI.Domain.Entities;

public class BudgetEntity : BaseEntity
{
    /// <summary>Budget limit.</summary>
    public decimal Budget { get; set; }
    public decimal Total { get; set; }
    public string CurrencyCode { get; set; } = "USD";
}
