namespace TravellerAI.Domain.ViewModels.Requests;

public class DeleteAccountRequest
{
    /// <summary>Current password to confirm the deletion.</summary>
    public string Password { get; set; }
}
