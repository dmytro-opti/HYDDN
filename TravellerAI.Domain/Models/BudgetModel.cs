namespace TravellerAI.Domain.Models;
// фільтр
public class BudgetModel
{
    public Guid Id { get; set; }
    public RecommendationModel Recommendation { get; set; }
    public decimal Budget { get; set; } //Limit
    public decimal Total { get; set; }
    public string CurrencyCode { get; set; } = "USD";
    public bool IsWithBudget => Total <= Budget;
    public decimal RemainingBudget => Budget - Total;
    public BalanceModel Ballance { get; set; }
}