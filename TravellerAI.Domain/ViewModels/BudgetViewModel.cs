namespace TravellerAI.Domain.ViewModels;

public class BudgetViewModel
{
    /// <summary>Budget limit.</summary>
    public decimal Budget { get; set; }
    /// <summary>Calculated costs of transports and booking.</summary>
    public decimal Total { get; set; }
    public decimal RemainingBudget { get; set; }
    public bool IsWithBudget { get; set; }
    public string CurrencyCode { get; set; }
}
