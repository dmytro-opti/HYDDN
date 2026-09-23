namespace TravellerAI.Domain.ViewModels;

/// <summary>
/// Status of a resource (trip, journey).
/// </summary>
public class StatusViewModel<TStatus> where TStatus : struct, Enum
{
    public Guid Id { get; set; }
    public TStatus Status { get; set; }
}
