using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;

namespace TravellerAI.Domain.ViewModels;

public class TransportViewModel
{
    public TransportType Type { get; set; }
    public string Company { get; set; }
    public decimal Price { get; set; }
    public SeatClass SeatClass { get; set; }
    public int SeatCount { get; set; }
    public int TotalBudget { get; set; }
}