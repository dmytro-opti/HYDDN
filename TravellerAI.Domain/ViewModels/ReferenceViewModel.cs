namespace TravellerAI.Domain.ViewModels;

/// <summary>
/// Link to a related object: id and display name.
/// </summary>
public class ReferenceViewModel
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
}
