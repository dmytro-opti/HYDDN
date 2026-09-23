namespace TravellerAI.Domain.Models;

/// <summary>
/// Lightweight link to a related object (id and display name).
/// </summary>
public class ReferenceModel
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
}
