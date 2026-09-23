using TravellerAI.Domain.Enums;

namespace TravellerAI.Domain.ViewModels;

public class JourneyProgressViewModel
{
    public Guid JourneyId { get; set; }
    public bool Approved { get; set; }
    public bool CanApprove { get; set; }
    /// <summary>Step the client should continue with; null when setup is complete.</summary>
    public JourneyStep? NextStep { get; set; }
    public List<JourneyStepViewModel> Steps { get; set; }
}

public class JourneyStepViewModel
{
    public JourneyStep Step { get; set; }
    public bool Completed { get; set; }
    public List<string> Errors { get; set; }
}
