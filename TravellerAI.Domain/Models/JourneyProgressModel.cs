using TravellerAI.Domain.Enums;

namespace TravellerAI.Domain.Models;

/// <summary>
/// Journey setup state: completed steps and what is still missing.
/// </summary>
public class JourneyProgressModel
{
    public Guid JourneyId { get; set; }
    public bool Approved { get; set; }
    public bool CanApprove => !Approved && Steps.All(s => s.Completed);
    /// <summary>First step which is not completed; null when everything is done.</summary>
    public JourneyStep? NextStep => Steps.FirstOrDefault(s => !s.Completed)?.Step;
    public List<JourneyStepModel> Steps { get; set; } = new();
}

public class JourneyStepModel
{
    public JourneyStep Step { get; set; }
    public bool Completed => Errors.Count == 0;
    public List<string> Errors { get; set; } = new();
}
